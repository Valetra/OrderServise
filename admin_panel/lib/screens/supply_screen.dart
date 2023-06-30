import 'package:admin_panel/models/category.dart';
import 'package:admin_panel/services/view_arguments.dart';
import 'package:admin_panel/models/supply.dart';
import 'package:admin_panel/services/remote_service.dart';
import 'package:admin_panel/widgets/scrollable_widget.dart';
import 'package:admin_panel/utils.dart';
import 'package:admin_panel/widgets/text_dialog_widget.dart';

import 'package:flutter/material.dart';
import 'package:flutter_guid/flutter_guid.dart';

class SuppliesScreen extends StatefulWidget {
  const SuppliesScreen({super.key});

  static const routeName = '/supplies';

  @override
  State<SuppliesScreen> createState() => _SuppliesScreenState();
}

class _SuppliesScreenState extends State<SuppliesScreen> {
  List<Supply> supplies = List.empty();
  List<Category> categories = List.empty();

  @override
  void initState() {
    initializeData();
    super.initState();
  }

  initializeData() async {
    supplies = await getSupplies();
    categories = await getCategories();

    setState(() {});
  }

  Future<List<Supply>> getSupplies() async {
    return await RemotesService().getSupplyList();
  }

  Future<List<Category>> getCategories() async {
    return await RemotesService().getCategoryList();
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
          backgroundColor: MaterialStateProperty.all(
              const Color.fromARGB(255, 108, 151, 243)),
          overlayColor: MaterialStateProperty.all(
              const Color.fromARGB(255, 82, 115, 185)),
        ),
        onPressed: () async {
          if (categories.isNotEmpty) {
            createNewSupply();
          } else {
            showDialog<String>(
              context: context,
              builder: (BuildContext context) => AlertDialog(
                backgroundColor: const Color.fromARGB(255, 184, 77, 77),
                title: const Text('Ошибка создания продукта'),
                content: const Text(
                    'Создайте хотя бы одну категорию, для создания продукта.'),
                actions: <Widget>[
                  TextButton(
                    onPressed: () => Navigator.pop(context, 'назад'),
                    child: const Text(
                      'назад',
                      style:
                          TextStyle(color: Color.fromARGB(255, 255, 255, 255)),
                    ),
                  ),
                ],
              ),
            );
          }
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
      'Удалить',
      'Опубликовано'
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
          "Удалить",
          "Опубликовано"
        ];

        Map<String, num> columnIndexes = {
          "nameColumn": 0,
          "priceColumn": 1,
          "cookTimeColumn": 2,
          "categoryColumn": 3,
          "deleteColumn": 4,
          "published": 5,
        };

        dynamic cellColor;

        Color defaultCellsColor(Set<MaterialState> states) {
          return const Color.fromARGB(255, 151, 208, 212);
        }

        Color notPublishedCellsColor(Set<MaterialState> states) {
          return const Color.fromARGB(255, 224, 167, 121);
        }

        if (supply.id == null) {
          cellColor = MaterialStateProperty.resolveWith(notPublishedCellsColor);
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
              } else if (index == columnIndexes["deleteColumn"]!) {
                return getDeleteSupplyCell(supply);
              } else {
                return getPuplishedCell(supply);
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

  DataCell getPuplishedCell(Supply supply) {
    ButtonStyle publishedButtonStyle = ButtonStyle(
      backgroundColor:
          MaterialStateProperty.all(const Color.fromARGB(255, 126, 214, 108)),
    );

    ButtonStyle notPublishedButtonStyle = ButtonStyle(
      backgroundColor:
          MaterialStateProperty.all(Color.fromARGB(255, 226, 108, 108)),
      overlayColor:
          MaterialStateProperty.all(const Color.fromARGB(255, 126, 214, 108)),
    );

    ButtonStyle buttonStyle;

    if (supply.id == null) {
      buttonStyle = notPublishedButtonStyle;
      return DataCell(
        ElevatedButton(
          style: buttonStyle,
          onPressed: () {
            publishSupply(supply);
          },
          child: const Icon(
            Icons.check,
            size: 22,
            color: Color.fromARGB(255, 0, 0, 0),
          ),
        ),
      );
    } else {
      buttonStyle = publishedButtonStyle;
      return DataCell(
        ElevatedButton(
          style: buttonStyle,
          onPressed: null,
          child: const Icon(
            Icons.check,
            size: 22,
            color: Color.fromARGB(255, 0, 0, 0),
          ),
        ),
      );
    }
  }

  DataCell getDeleteSupplyCell(Supply supply) {
    return DataCell(
      ElevatedButton(
        style: ButtonStyle(
          backgroundColor: MaterialStateProperty.all(
              const Color.fromARGB(255, 255, 255, 255)),
          overlayColor:
              MaterialStateProperty.all(const Color.fromARGB(255, 255, 90, 90)),
        ),
        onPressed: () {
          if (supply.id == null) {
            deleteLocalSupply(supply);
          } else {
            deleteRemoteSupply(supply);
          }
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
                    categories.where((c) => c.name == newValue).first.id!;
                editSupplyCategoryId(supply);
              });
            },
            items:
                categories.map<DropdownMenuItem<String>>((Category category) {
              return DropdownMenuItem<String>(
                value: category.name,
                child: Text(category.name),
              );
            }).toList(),
          ),
        ),
      ),
    );
  }

  deleteLocalSupply(Supply supply) {
    setState(() {
      supplies.remove(supply);
    });
  }

  Future deleteRemoteSupply(Supply supply) async {
    deleteSupply(Guid supplyId) async {
      return await RemotesService().deleteSupply(supplyId);
    }

    setState(() {
      deleteSupply(supply.id!);
      supplies.remove(supply);
    });
  }

  createNewSupply() {
    Supply newSupply = Supply(
        name: "Новый продукт",
        price: 0,
        cookingTime: "00:00:00",
        categoryId: categories.first.id!);

    supplies.add(newSupply);

    setState(() {});
  }

  publishSupply(Supply supply) async {
    await RemotesService().createSupply(supply);
    supplies = await getSupplies();
    setState(() {});
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
      if (supply.id != null) {
        return await RemotesService().updateSupply(supply);
      }
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
      if (supply.id != null) {
        return await RemotesService().updateSupply(editSupply);
      }
    }

    setState(() {
      updateOrder(editSupply);

      supplies = supplies.map((supply) {
        final isEditedSupply = supply == editSupply;

        return isEditedSupply
            ? supply.copy(categoryId: editSupply.categoryId)
            : supply;
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
      if (supply.id != null) {
        return await RemotesService().updateSupply(supply);
      }
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
      if (supply.id != null) {
        return await RemotesService().updateSupply(supply);
      }
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
