import 'package:admin_panel/models/category.dart';
import 'package:admin_panel/services/view_arguments.dart';
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
    //Fetch data from API
    getSupplies();
    getCategories();

    super.initState();
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
    final args = ModalRoute.of(context)!.settings.arguments as ViewArguments;

    return Scaffold(
      appBar: AppBar(
        backgroundColor: args.backgroundColor,
        title: Text(args.title),
      ),
      body: ScrollableWidget(child: buildDataTable()),
      floatingActionButton: ElevatedButton(
        style: ButtonStyle(
          backgroundColor:
              MaterialStateProperty.all(Color.fromARGB(255, 108, 151, 243)),
          overlayColor:
              MaterialStateProperty.all(Color.fromARGB(255, 82, 115, 185)),
        ),
        onPressed: () {
          createNewSupply();
        },
        child: const Icon(
          Icons.add,
          size: 22,
          color: Color.fromARGB(255, 255, 255, 255),
        ),
      ),
      floatingActionButtonLocation: FloatingActionButtonLocation.startFloat,
    );
  }

  Widget buildDataTable() {
    final columns = [
      'Название',
      'Цена',
      'Время приготовления',
      'Категория',
      'Удалить'
    ];

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
          supply.categoryId,
          "Удалить"
        ];

        Map<String, num> columnIndexes = {
          "nameColumn": 0,
          "priceColumn": 1,
          "cookTimeColumn": 2,
          "categoryColumn": 3,
          "deleteColumn": 4,
        };

        var cellColor;

        Color defaultCellsColor(Set<MaterialState> states) {
          return Color.fromARGB(255, 151, 208, 212);
        }

        Color newProductCellsColor(Set<MaterialState> states) {
          return Color.fromARGB(255, 224, 167, 121);
        }

        if (supply.name == "Новый продукт") {
          cellColor = MaterialStateProperty.resolveWith(newProductCellsColor);
        } else {
          cellColor = MaterialStateProperty.resolveWith(defaultCellsColor);
        }

        return DataRow(
            cells: Utils.modelBuilder(cells, (index, cell) {
              if (index == columnIndexes["nameColumn"]! ||
                  index == columnIndexes["priceColumn"]! ||
                  index == columnIndexes["cookTimeColumn"]!) {
                return getEditSupplyCell(supply, cell, index);
              } else if (index == columnIndexes["categoryColumn"]!) {
                return getDropdownCategoriesCell(supply);
              } else {
                return getDeleteSupplyCell(supply);
              }
            }),
            color: cellColor);
      }).toList();

  DataCell getEditSupplyCell(Supply supply, Object cell, int rowIndex) {
    return DataCell(
      Text('$cell'),
      showEditIcon: true,
      onTap: () {
        switch (rowIndex) {
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
  }

  DataCell getDeleteSupplyCell(Supply supply) {
    return DataCell(
      ElevatedButton(
        style: ButtonStyle(
          backgroundColor:
              MaterialStateProperty.all(Color.fromARGB(255, 255, 255, 255)),
          overlayColor:
              MaterialStateProperty.all(Color.fromARGB(255, 255, 90, 90)),
        ),
        onPressed: () {
          deleteSupply(supply);
        },
        child: const Icon(
          Icons.delete,
          size: 22,
          color: Color.fromARGB(255, 0, 0, 0),
        ),
      ),
    );
  }

  DataCell getDropdownCategoriesCell(Supply supply) {
    return DataCell(
      TextField(
        decoration: InputDecoration(
          labelText: 'Select an item',
          suffixIcon: DropdownButtonFormField(
            dropdownColor: const Color.fromARGB(255, 190, 190, 190),
            value:
                categories.where((c) => c.id == supply.categoryId).first.name,
            onChanged: (newValue) {
              setState(() {
                supply.categoryId =
                    categories.where((c) => c.name == newValue).first.id;
                editSupplyCategoryId(supply);
              });
            },
            items: categoryNames.map<DropdownMenuItem<String>>((String value) {
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

  Future deleteSupply(Supply supply) async {
    deleteSupply(Guid supplyId) async {
      return await RemotesService().deleteSupply(supplyId);
    }

    setState(() {
      deleteSupply(supply.id!);
    });
  }

  Future createNewSupply() async {
    createSupply() async {
      Supply newSupply = Supply(
          name: "Новый продукт",
          price: 0,
          cookingTime: "00:00:00",
          categoryId:
              categories.where((c) => c.name == "Нет категории").first.id);

      try {
        await RemotesService().createSupply(newSupply);
      } catch (e) {
        return showDialog<String>(
          context: context,
          builder: (BuildContext context) => AlertDialog(
            backgroundColor: Color.fromARGB(255, 184, 77, 77),
            title: const Text('Ошибка создания продукта'),
            content: const Text(
                'Переименуйте "Новый продукт", после чего высможете создать ещё один новый продукт.'),
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
      }
    }

    setState(() {
      createSupply();
    });
  }

  Future editSupplyName(Supply editedSupply) async {
    final name = await showTextDialog(
      context,
      title: "Измените название блюда",
      value: editedSupply.name,
    );

    updateSupply(Supply supply) async {
      Supply updatedSupply;

      if (name == null) {
        return null;
      }

      try {
        supply.name = name;
        updatedSupply = await RemotesService().updateSupply(supply);
      } catch (e) {
        return showDialog<String>(
          context: context,
          builder: (BuildContext context) => AlertDialog(
            backgroundColor: Color.fromARGB(255, 184, 77, 77),
            title: const Text('Ошибка переименования продукта'),
            content: const Text(
                'Повторение имени продукта исключено. Выберите другое имя.'),
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
      }

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

  Future editSupplyCategoryId(Supply editSupply) async {
    updateOrder(Supply supply) async {
      Supply updatedSupply = await RemotesService().updateSupply(editSupply);
      return updatedSupply;
    }

    setState(() {
      updateOrder(editSupply);
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
