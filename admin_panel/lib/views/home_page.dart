import 'package:admin_panel/Models/order.dart';
import 'package:admin_panel/services/remote_service.dart';

import 'package:flutter/material.dart';

class HomePage extends StatefulWidget {
  const HomePage({super.key});

  @override
  State<HomePage> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
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
    return Scaffold(
      appBar: AppBar(
        title: const Text("Orders"),
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
      ),
    );
  }
}

// ElevatedButton(
//   child: const Text('Блюда'),
//   onPressed: () {
//     Navigator.push(
//       context,
//       MaterialPageRoute(builder: (context) => const SecondRoute()),
//     );
//   },
// )

class SecondRoute extends StatelessWidget {
  const SecondRoute({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Блюда'),
      ),
      body: Center(
        child: ElevatedButton(
          onPressed: () {
            Navigator.pop(context);
          },
          child: const Text('К заказам'),
        ),
      ),
    );
  }
}
