import 'package:admin_panel/Models/order.dart';
import 'package:admin_panel/services/remote_service.dart';
import 'package:admin_panel/services/screen_arguments.dart';
import 'package:admin_panel/ScrollableWidget.dart';

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

  List<DataRow> getRows(List<Order> orders) => orders.map((Order order) {
        final cells = [order.id, order.status, order.payed];

        return DataRow(cells: getCells(cells));
      }).toList();

  List<DataCell> getCells(List<dynamic> cells) =>
      cells.map((data) => DataCell(Text('$data'))).toList();
}

//   @override
//   Widget build(BuildContext context) {
//     final args = ModalRoute.of(context)!.settings.arguments as ScreenArguments;

//     return Scaffold(
//         appBar: AppBar(
//           backgroundColor: args.backgroundColor,
//           title: Text(args.title),
//         ),
//         body: Visibility(
//           visible: isLoaded,
//           replacement: const Center(
//             child: CircularProgressIndicator(),
//           ),
//           child: ListView.builder(
//             itemCount: orders?.length,
//             itemBuilder: (context, index) {
//               return Container(
//                 child: Text(
//                   '''Номер заказа: ${index + 1}
// Статус оплаты: ${orders![index].payed.toString()}
// Статус заказа: ${orders![index].status}
//               ''',
//                   style: const TextStyle(fontWeight: FontWeight.bold),
//                 ),
//               );
//             },
//           ),
//         ));
//   }
