import 'package:admin_panel/views/order_screen.dart';
import 'package:admin_panel/services/screen_arguments.dart';
import 'package:admin_panel/views/supply_screen.dart';

import 'package:flutter/material.dart';

class HomeScreen extends StatelessWidget {
  const HomeScreen({super.key});

  @override
  Widget build(BuildContext context) {
    final ButtonStyle style = TextButton.styleFrom(
        foregroundColor: Theme.of(context).colorScheme.onPrimary);

    const String orderTitle = "Заказы";
    const String supplyTitle = "Блюда";

    const Color bgColor = Color(0xff201F23);

    return Scaffold(
      appBar: AppBar(
        backgroundColor: bgColor,
        title: Row(children: <Widget>[
          const Text("Админ панель"),
          TextButton(
            style: style,
            onPressed: () {
              Navigator.pushNamed(
                context,
                OrdersScreen.routeName,
                arguments: ScreenArguments(orderTitle,
                    "TODO: add order screen with data here", bgColor),
              );
            },
            child: const Text(orderTitle),
          ),
          TextButton(
            style: style,
            onPressed: () {
              Navigator.pushNamed(
                context,
                SuppliesScreen.routeName,
                arguments: ScreenArguments(supplyTitle,
                    "TODO: add supply screen with data here", bgColor),
              );
            },
            child: const Text('Блюда'),
          ),
        ]),
      ),
      body: const Center(child: Text("TODO: add home screen with data here")),
    );
  }
}
