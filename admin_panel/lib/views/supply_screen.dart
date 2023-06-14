import 'package:admin_panel/models/category.dart';
import 'package:admin_panel/services/screen_arguments.dart';
import 'package:admin_panel/models/supply.dart';
import 'package:admin_panel/services/remote_service.dart';
import 'package:admin_panel/widgets/scrollable_widget.dart';
import 'package:admin_panel/utils.dart';
import 'package:admin_panel/widgets/text_dialog_widget.dart';
import 'package:flutter_guid/flutter_guid.dart';

import 'package:flutter/material.dart';
import 'package:flutter/services.dart';

class SuppliesScreen extends StatefulWidget {
  const SuppliesScreen({super.key});

  static const routeName = '/supplies';
  @override
  State<SuppliesScreen> createState() => _SuppliesScreenState();
}

class _SuppliesScreenState extends State<SuppliesScreen> {
  List<Supply> supplies = List.empty();
  List<Category> categories = List.empty();
  List<String> categoryNames = List.empty(growable: true);

  bool isLoaded = false;

  @override
  void initState() {
    super.initState();

    //Fetch data from API
    getSupplies();
    getCategories();
  }

  getSupplies() async {
    supplies = await RemotesService().getSupplyList();

    setState(() {
      isLoaded = true;
    });
  }

  getCategories() async {
    categories = await RemotesService().getCategoryList();

    for (var category in categories) {
      categoryNames.add(category.name);
    }

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
            if (index != 3) {
              return DataCell(
                Text('$cell'),
                showEditIcon: true,
                onTap: () {
                  switch (index) {
                    case 0:
                      editSupplyName(supply);
                      break;
                    case 1:
                      editSupplyPrice(supply);
                      break;
                    case 2:
                      editSupplyCookingTime(supply);
                      break;
                  }
                },
              );
            } else {
              return DataCell(
                TextField(
                  decoration: InputDecoration(
                    labelText: 'Select an item',
                    suffixIcon: DropdownButtonFormField(
                      dropdownColor: const Color.fromARGB(255, 190, 190, 190),
                      value: categories
                          .where((c) => c.id == supply.categoryId)
                          .first
                          .name,
                      onChanged: (newValue) {
                        setState(() {
                          supply.categoryId = categories
                              .where((c) => c.name == newValue)
                              .first
                              .id;
                          editSupplyCategoryId(supply);
                        });
                      },
                      items: categoryNames
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

  Future editSupplyCategoryId(Supply editSupply) async {
    updateOrder(Supply supply) async {
      Supply updatedSupply = await RemotesService().updateSupply(editSupply);
      return updatedSupply;
    }

    setState(() {
      updateOrder(editSupply);
    });
  }

  Future editSupplyName(Supply editedSupply) async {
    final name = await showTextDialog(
      context,
      title: "Измените название блюда",
      value: editedSupply.name,
    );
    updateSupply(Supply supply) async {
      if (name == null) {
        return null;
      }
      supply.name = name;
      Supply updatedSupply = await RemotesService().updateSupply(supply);
      return updatedSupply;
    }

    setState(() {
      updateSupply(editedSupply);

      supplies = supplies.map((supply) {
        final isEditedSupply = supply == editedSupply;

        return isEditedSupply ? supply.copy(name: name) : supply;
      }).toList();
    });
  }

  Future editSupplyPrice(Supply editedSupply) async {
    final price = await showTextDialog(
      context,
      title: "Измените цену блюда",
      value: editedSupply.price.toString(),
    );
    updateSupply(Supply supply) async {
      if (price == null) {
        return null;
      }
      supply.price = int.parse(price);
      Supply updatedSupply = await RemotesService().updateSupply(supply);
      return updatedSupply;
    }

    setState(() {
      updateSupply(editedSupply);
      if (price != null) {
        supplies = supplies.map((supply) {
          final isEditedSupply = supply == editedSupply;

          return isEditedSupply ? supply.copy(price: int.parse(price)) : supply;
        }).toList();
      }
    });
  }

  Future editSupplyCookingTime(Supply editedSupply) async {
    final cookingTime = await showTextDialog(
      context,
      title: "Измените время приготовления блюда",
      value: editedSupply.cookingTime,
    );
    updateSupply(Supply supply) async {
      if (cookingTime == null) {
        return null;
      }
      supply.cookingTime = cookingTime;
      Supply updatedSupply = await RemotesService().updateSupply(supply);
      return updatedSupply;
    }

    setState(() {
      updateSupply(editedSupply);

      supplies = supplies.map((supply) {
        final isEditedSupply = supply == editedSupply;

        return isEditedSupply ? supply.copy(cookingTime: cookingTime) : supply;
      }).toList();
    });
  }
}
