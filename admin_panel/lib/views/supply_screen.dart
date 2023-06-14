import 'package:admin_panel/services/screen_arguments.dart';
import 'package:admin_panel/models/supply.dart';
import 'package:admin_panel/services/remote_service.dart';
import 'package:admin_panel/widgets/scrollable_widget.dart';
import 'package:admin_panel/utils.dart';

import 'package:flutter/material.dart';

class SuppliesScreen extends StatefulWidget {
  const SuppliesScreen({super.key});

  static const routeName = '/supplies';
  @override
  State<SuppliesScreen> createState() => _SuppliesScreenState();
}

class _SuppliesScreenState extends State<SuppliesScreen> {
  List<Supply> supplies = List.empty();
  bool isLoaded = false;

  @override
  void initState() {
    super.initState();

    //Fetch data from API
    getSupplies();
  }

  getSupplies() async {
    supplies = await RemotesService().getSupplyList();

    setState(() {
      isLoaded = true;
    });
  }

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
    final columns = ['Название', 'Цена', 'Время приготовления', 'Категория'];

    return DataTable(
      columns: getColumns(columns),
      rows: getRows(supplies),
    );
  }

  List<DataColumn> getColumns(List<String> columns) => columns
      .map((String column) => DataColumn(
            label: Text(column),
          ))
      .toList();

  List<DataRow> getRows(List<Supply> supplies) => supplies.map((Supply supply) {
        var cells = [
          supply.name,
          supply.price,
          supply.cookingTime,
          supply.categoryId
        ];

        return DataRow(
          cells: Utils.modelBuilder(cells, (index, cell) {
            return DataCell(Text('$cell'));
          }),
        );
      }).toList();

  // Future editSupply(Supply editedSupply) async {
  //   updateSupply(Supply order) async {
  //     Supply updatedSupply = await RemotesService().updateSupply(editedSupply);
  //     return updatedSupply;
  //   }

  //   setState(() {
  //     updateSupply(editedSupply);
  //   });
  // }
}
