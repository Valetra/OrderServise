import 'package:admin_panel/Models/order.dart';
import 'package:admin_panel/services/remote_service.dart';
import 'package:admin_panel/services/screen_arguments.dart';
import 'package:admin_panel/widgets/scrollable_widget.dart';
import 'package:admin_panel/utils.dart';

import 'package:flutter/material.dart';

class OrdersScreen extends StatefulWidget {
  const OrdersScreen({super.key});

  static const routeName = '/orders';

  @override
  State<OrdersScreen> createState() => _OrdersScreenState();
}

class _OrdersScreenState extends State<OrdersScreen> {
  List<Order> orders = List.empty();
  bool isLoaded = false;

  @override
  void initState() {
    super.initState();

    //Fetch data from API
    getOrders();
  }

  getOrders() async {
    orders = await RemotesService().getOrderList();

    setState(() {
      isLoaded = true;
    });
  }

//
  @override
  Widget build(BuildContext context) {
    final args = ModalRoute.of(context)!.settings.arguments as ScreenArguments;

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
    mapIterator = 1;
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

  List<String> statuses = [
    "Unconfirmed",
    "Confirmed",
    "In progres",
    "Done",
    "Cancelled"
  ];

  int mapIterator = 1;
  List<DataRow> getRows(List<Order> orders) => orders.map((Order order) {
        var cells = [order.id, order.status, order.payed];

        return DataRow(
          cells: Utils.modelBuilder(cells, (index, cell) {
            if (index == 0) {
              return DataCell(Text('${mapIterator++}'));
            } else if (index == 2) {
              return DataCell(Text('$cell'));
            } else {
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
                      items: statuses
                          .map<DropdownMenuItem<String>>((String value) {
                        return DropdownMenuItem<String>(
                          value: value,
                          child: Text(value),
                        );
                      }).toList(),
                    ),
                  ),
                ),
              );
            }
          }),
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
