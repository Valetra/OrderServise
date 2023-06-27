import 'package:admin_panel/screens/category_screen.dart';
import 'package:admin_panel/screens/home_screen.dart';
import 'package:admin_panel/screens/order_screen.dart';
import 'package:admin_panel/screens/supply_screen.dart';

import 'package:flutter/material.dart';

void main() => runApp(const MyApp());

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      routes: {
        OrdersScreen.routeName: (context) => const OrdersScreen(),
        SuppliesScreen.routeName: (context) => const SuppliesScreen(),
        CategoriesScreen.routeName: (context) => const CategoriesScreen(),
      },
      title: 'Админ панель',
      home: const HomeScreen(),
    );
  }
}
