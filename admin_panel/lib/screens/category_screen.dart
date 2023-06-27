import 'package:admin_panel/models/category.dart';
import 'package:admin_panel/services/remote_service.dart';
import 'package:admin_panel/services/view_arguments.dart';
import 'package:admin_panel/widgets/scrollable_widget.dart';
import 'package:admin_panel/widgets/text_dialog_widget.dart';

import 'package:flutter/material.dart';
import 'package:flutter_guid/flutter_guid.dart';

class CategoriesScreen extends StatefulWidget {
  const CategoriesScreen({super.key});

  static const routeName = '/categories';

  @override
  State<CategoriesScreen> createState() => _CategoriesScreenState();
}

class _CategoriesScreenState extends State<CategoriesScreen> {
  List<Category> categories = List.empty();

  @override
  void initState() {
    initializeData();
    super.initState();
  }

  initializeData() async {
    categories = await getCategories();

    setState(() {});
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
          createNewCategory();
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
    final columns = ['Название', 'Удалить'];

    return DataTable(
      columns: getColumns(columns),
      rows: getRows(categories),
    );
  }

  List<DataColumn> getColumns(List<String> columns) => columns
      .map((String column) => DataColumn(
            label: Text(column),
          ))
      .toList();

  List<DataRow> getRows(List<Category> category) =>
      category.map((Category category) {
        dynamic cellColor;

        Color namedCellsColor(Set<MaterialState> states) {
          return const Color.fromARGB(255, 151, 212, 169);
        }

        Color notNamedCellsColor(Set<MaterialState> states) {
          return const Color.fromARGB(255, 224, 183, 148);
        }

        if (category.name == "Нет категории") {
          cellColor = MaterialStateProperty.resolveWith(notNamedCellsColor);
        } else {
          cellColor = MaterialStateProperty.resolveWith(namedCellsColor);
        }

        return DataRow(
          cells: [
            DataCell(
              Text(category.name),
              showEditIcon: true,
              onTap: () {
                editCategoryName(category);
              },
            ),
            DataCell(
              ElevatedButton(
                style: ButtonStyle(
                  backgroundColor: MaterialStateProperty.all(
                      const Color.fromARGB(255, 255, 255, 255)),
                  overlayColor: MaterialStateProperty.all(
                      const Color.fromARGB(255, 255, 90, 90)),
                ),
                onPressed: () {
                  deleteCategory(category);
                },
                child: const Icon(
                  Icons.delete,
                  size: 22,
                  color: Color.fromARGB(255, 0, 0, 0),
                ),
              ),
            )
          ],
          color: cellColor,
        );
      }).toList();

  Future deleteCategory(Category category) async {
    deleteCategory(Guid categoryId) async {
      return await RemotesService().deleteCategory(categoryId);
    }

    setState(() {
      deleteCategory(category.id!);
      categories.remove(category);
    });
  }

  createNewCategory() async {
    Category newCategory = Category(name: "Нет категории");

    createCategory() async {
      try {
        await RemotesService().createCategory(newCategory);
      } catch (e) {
        return showDialog<String>(
          context: context,
          builder: (BuildContext context) => AlertDialog(
            backgroundColor: const Color.fromARGB(255, 184, 77, 77),
            title: const Text('Ошибка создания продукта'),
            content: const Text(
                'Переименуйте "Нет категории", после чего высможете создать ещё один новый продукт.'),
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

    await createCategory();
    categories = await getCategories();

    setState(() {});
  }

  Future editCategoryName(Category editedCategory) async {
    final name = await showTextDialog(
      context,
      title: "Измените название категории",
      value: editedCategory.name,
    );

    updateCategory(Category category) async {
      Category updatedCategory;

      if (name == null) {
        return null;
      }

      try {
        category.name = name;
        updatedCategory = await RemotesService().updateCategory(category);
      } catch (e) {
        return showDialog<String>(
          context: context,
          builder: (BuildContext context) => AlertDialog(
            backgroundColor: const Color.fromARGB(255, 184, 77, 77),
            title: const Text('Ошибка переименования категории'),
            content: const Text(
                'Повторение имени категории исключено. Выберите другое имя.'),
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

      return updatedCategory;
    }

    setState(() {
      updateCategory(editedCategory);

      categories = categories.map((category) {
        final isEditedCategory = category == editedCategory;

        return isEditedCategory ? category.copy(name: name) : category;
      }).toList();
    });
  }
}
