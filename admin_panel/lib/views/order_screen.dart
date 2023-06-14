import 'package:admin_panel/Models/order.dart';
import 'package:admin_panel/services/remote_service.dart';
import 'package:admin_panel/services/screen_arguments.dart';
import 'package:admin_panel/widgets/scrollable_widget.dart';
import 'package:admin_panel/widgets/text_dialog_widget.dart';
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

        return DataRow(
          cells: Utils.modelBuilder(cells, (index, cell) {
            final showEditIcon = index == 1;

            return DataCell(Text('$cell'), showEditIcon: showEditIcon,
                onTap: () {
              switch (index) {
                case 1:
                  editOrderStatus(order);
                  ScrollableWidget(child: buildDataTable());
                  break;
              }
            });
          }),
        );
      }).toList();

  Future editOrderStatus(Order editOrder) async {
    final status = await showTextDialog(
      context,
      title: 'Измените статус заказа',
      value: editOrder.status,
    );

    updateOrder(Order order) async {
      Order updatedOrder = await RemotesService().updateOrder(order, status);
      return updatedOrder;
    }

    setState(() {
      orders = orders.map((order) {
        final isEditedOrder = order == editOrder;

        return isEditedOrder ? order.copy(status: status) : order;
      }).toList();

      updateOrder(editOrder);
    });
  }
}
