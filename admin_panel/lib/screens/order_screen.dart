import 'package:admin_panel/Models/order.dart';
import 'package:admin_panel/services/remote_service.dart';
import 'package:admin_panel/services/view_arguments.dart';
import 'package:admin_panel/widgets/scrollable_widget.dart';
import 'package:admin_panel/utils.dart';

import 'package:flutter/material.dart';

class OrderStatus {
  final String value;
  final String label;

  OrderStatus(this.value, this.label) {}
}

class OrdersScreen extends StatefulWidget {
  const OrdersScreen({super.key});

  static const routeName = '/orders';

  @override
  State<OrdersScreen> createState() => _OrdersScreenState();
}

class _OrdersScreenState extends State<OrdersScreen> {
  List<Order> orders = List.empty();

  @override
  void initState() {
    super.initState();

    //Fetch data from API
    getOrders();
  }

  getOrders() async {
    List<Order> fetchedOrders = await RemotesService().getOrderList();

    setState(() {
      orders = fetchedOrders;
    });
  }

//
  @override
  Widget build(BuildContext context) {
    final args = ModalRoute.of(context)!.settings.arguments as ViewArguments;

    return Scaffold(
      appBar: AppBar(
        backgroundColor: args.backgroundColor,
        title: Text(args.title),
      ),
      body: ScrollableWidget(child: buildDataTable()),
    );
  }

  Widget buildDataTable() {
    final columns = ['Номер заказа', 'Статус заказа', 'Оплата'];

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
    new OrderStatus("Unconfirmed", "Не подтверждён"),
    new OrderStatus("Confirmed", "Подтверждён"),
    new OrderStatus("In progres", "В процессе"),
    new OrderStatus("Done", "Готов"),
    new OrderStatus("Cancelled", "Отменён"),
  ];

  List<DataRow> getRows(List<Order> orders) => orders.map((Order order) {
        var index = orders.indexOf(order) + 1;

        return DataRow(
          cells: [
            DataCell(
              Text('$index'),
              onTap: () {
                //Allert window with order content
                showDialog<String>(
                  context: context,
                  builder: (BuildContext context) => AlertDialog(
                    backgroundColor: Color.fromARGB(255, 187, 196, 111),
                    title: Text(
                        'Содержимое заказа №$index'), //Как вытащить номер заказа?!
                    content: const Text('Здесь должно быть содержимое заказа.'),
                    actions: <Widget>[
                      TextButton(
                        onPressed: () => Navigator.pop(context, 'назад'),
                        child: const Text(
                          'назад',
                          style: TextStyle(
                              color: Color.fromARGB(255, 255, 255, 255)),
                        ),
                      ),
                    ],
                  ),
                );
              },
            ),
            DataCell(
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
                    items: statuses
                        .map<DropdownMenuItem<String>>((OrderStatus status) {
                      return DropdownMenuItem<String>(
                        value: status.value,
                        child: Text(status.label),
                      );
                    }).toList(),
                  ),
                ),
              ),
            ),
            DataCell(Text(order.payed ? 'Оплачено' : 'Не оплачено')),
          ],
        );
      }).toList();

  Future editOrderStatus(Order editOrder) async {
    updateOrder(Order order) async {
      Order updatedOrder = await RemotesService().updateOrder(editOrder);
      return updatedOrder;
    }

    setState(() {
      updateOrder(editOrder);
    });
  }
}
