import 'package:admin_panel/Models/order.dart';
import 'package:admin_panel/services/remote_service.dart';

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
      },
      title: 'Админ панель',
      home: const HomeScreen(),
    );
  }
}

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

class OrdersScreen extends StatefulWidget {
  const OrdersScreen({super.key});

  static const routeName = '/orders';

  @override
  State<OrdersScreen> createState() => _OrdersScreenState();
}

class _OrdersScreenState extends State<OrdersScreen> {
  List<Order>? orders;
  bool isLoaded = false;

  @override
  void initState() {
    super.initState();

    //Fetch data from API
    getData();
  }

  getData() async {
    orders = await RemotesService().getOrders();

    if (orders != null) {
      setState(() {
        isLoaded = true;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    final args = ModalRoute.of(context)!.settings.arguments as ScreenArguments;

    return Scaffold(
        appBar: AppBar(
          backgroundColor: args.backgroundColor,
          title: Text(args.title),
        ),
        body: Visibility(
          visible: isLoaded,
          replacement: const Center(
            child: CircularProgressIndicator(),
          ),
          child: ListView.builder(
            itemCount: orders?.length,
            itemBuilder: (context, index) {
              return Container(
                child: Text(
                  '''Номер заказа: ${index + 1} 
Статус оплаты: ${orders![index].payed.toString()}
Статус заказа: ${orders![index].status}
              ''',
                  style: const TextStyle(fontWeight: FontWeight.bold),
                ),
              );
            },
          ),
        ));
  }
}

class SuppliesScreen extends StatelessWidget {
  const SuppliesScreen({super.key});

  static const routeName = '/supplies';

  @override
  Widget build(BuildContext context) {
    final args = ModalRoute.of(context)!.settings.arguments as ScreenArguments;

    return Scaffold(
      appBar: AppBar(
        backgroundColor: args.backgroundColor,
        title: Text(args.title),
      ),
      body: Center(
        child: Text(args.message),
      ),
    );
  }
}

class ScreenArguments {
  final String title;
  final String message;
  final Color backgroundColor;

  ScreenArguments(this.title, this.message, this.backgroundColor);
}
