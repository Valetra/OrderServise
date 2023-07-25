import 'package:admin_panel/Models/order.dart';
import 'package:admin_panel/models/supply.dart';
import 'package:admin_panel/services/remote_service.dart';
import 'package:admin_panel/services/view_arguments.dart';
import 'package:admin_panel/widgets/scrollable_widget.dart';

import 'package:intl/intl.dart';
import 'package:flutter/material.dart';
import 'package:web_socket_channel/web_socket_channel.dart';

class OrderStatus {
  final String value;
  final String label;

  OrderStatus(this.value, this.label);
}

class OrdersScreen extends StatefulWidget {
  const OrdersScreen({super.key});

  static const routeName = '/orders';

  @override
  State<OrdersScreen> createState() => _OrdersScreenState();
}

class _OrdersScreenState extends State<OrdersScreen> {
  final _channel = WebSocketChannel.connect(
    Uri.parse('ws://localhost:5132/ws'),
  );

  List<Order> orders = List.empty();

  @override
  void initState() {
    super.initState();

    getOrders();
  }

  getOrders() async {
    List<Order> fetchedOrders = await RemotesService().getOrderList();

    fetchedOrders.sort((a, b) {
      //sorting in ascending order
      return a.createDateTime.compareTo(b.createDateTime);
    });

    setState(() {
      orders = fetchedOrders;
    });
  }

  @override
  Widget build(BuildContext context) {
    final args = ModalRoute.of(context)!.settings.arguments as ViewArguments;

    return Scaffold(
      appBar: AppBar(
        backgroundColor: args.backgroundColor,
        title: Text(args.title),
      ),
      body: ScrollableWidget(
          child: StreamBuilder(
        stream: _channel.stream,
        builder: (context, snapshot) {
          _channel.sink.add("Handshake");
          if (snapshot.hasData) {
            orders = orderListFromJson(snapshot.data);
            orders.sort((a, b) {
              return a.createDateTime.compareTo(b.createDateTime);
            });
          }
          return buildDataTable();
        },
      )),
    );
  }

  Widget buildDataTable() {
    final columns = ['Номер', 'Статус', 'Оплата', 'Дата и время', 'Содержимое'];

    return DataTable(
      columns: getColumns(columns),
      rows: getRows(orders),
    );
  }

  List<DataColumn> getColumns(List<String> columns) => columns
      .map((String column) => DataColumn(
            label: Text(column),
          ))
      .toList();

  List<OrderStatus> statuses = [
    OrderStatus("Unconfirmed", "Не подтверждён"),
    OrderStatus("Confirmed", "Подтверждён"),
    OrderStatus("In progress", "В процессе"),
    OrderStatus("Done", "Готов"),
    OrderStatus("Cancelled", "Отменён"),
  ];

  List<DataRow> getRows(List<Order> orders) => orders.map((Order order) {
        dynamic cellColor;

        Color payedCellsColor(Set<MaterialState> states) {
          return const Color.fromARGB(255, 151, 212, 169);
        }

        Color notPayedCellsColor(Set<MaterialState> states) {
          return const Color.fromARGB(255, 224, 183, 148);
        }

        if (!order.payed) {
          cellColor = MaterialStateProperty.resolveWith(notPayedCellsColor);
        } else {
          cellColor = MaterialStateProperty.resolveWith(payedCellsColor);
        }

        return DataRow(
          cells: [
            DataCell(Text('${order.number}')),
            getOrderStatuCell(order),
            DataCell(Text(order.payed ? 'Оплачено' : 'Не оплачено')),
            getOrderDateTime(order),
            getOrderContentCell(order),
          ],
          color: cellColor,
        );
      }).toList();

  DataCell getOrderDateTime(order) {
    DateFormat dateFormatter = DateFormat('dd.MM.yyyy');
    DateFormat timeFormatter = DateFormat('Hms');

    String orderDate = dateFormatter.format(order.createDateTime.toLocal());
    String orderTime = timeFormatter.format(order.createDateTime.toLocal());

    return DataCell(Text('$orderDate\n$orderTime'));
  }

  DataCell getOrderContentCell(Order order) {
    return DataCell(
      const Center(
        child: Icon(
          Icons.format_list_bulleted,
          size: 22,
          color: Color.fromARGB(255, 78, 76, 76),
        ),
      ),
      onTap: () async {
        List<Supply> orderSupplies =
            await RemotesService().getOrderSupplies(order.id);

        String supplies = '';
        int totalPrice = 0;

        for (var supply in orderSupplies) {
          supplies += '${supply.name} - ${supply.count}шт\n';
          totalPrice += supply.price * supply.count as int;
        }
        supplies += 'Итого: $totalPrice₽';
        //Воспользоваться Future builder: https://api.flutter.dev/flutter/widgets/FutureBuilder-class.html
        showDialog<String>(
          context: context,
          builder: (BuildContext context) => AlertDialog(
            backgroundColor: const Color.fromARGB(255, 187, 196, 111),
            title: Text('Содержимое заказа №${order.number}'),
            content: Text(supplies),
            actions: <Widget>[
              TextButton(
                onPressed: () => Navigator.pop(context, 'назад'),
                child: const Text(
                  'назад',
                  style: TextStyle(color: Color.fromARGB(255, 255, 255, 255)),
                ),
              ),
            ],
          ),
        );
      },
    );
  }

  DataCell getOrderStatuCell(Order order) {
    return DataCell(
      TextField(
        decoration: InputDecoration(
          labelText: 'Select an item',
          suffixIcon: DropdownButtonFormField(
            dropdownColor: const Color.fromARGB(255, 190, 190, 190),
            value: order.status,
            onChanged: (newValue) {
              setState(() {
                order.status = newValue.toString();
                editOrderStatus(order);
              });
            },
            items: statuses.map<DropdownMenuItem<String>>((OrderStatus status) {
              return DropdownMenuItem<String>(
                value: status.value,
                child: Text(status.label),
              );
            }).toList(),
          ),
        ),
      ),
    );
  }

  Future editOrderStatus(Order editOrder) async {
    updateOrder(Order order) async {
      Order updatedOrder = await RemotesService().updateOrder(editOrder);
      return updatedOrder;
    }

    setState(() {
      updateOrder(editOrder);
    });
  }

  @override
  void dispose() {
    _channel.sink.close();
    super.dispose();
  }
}
