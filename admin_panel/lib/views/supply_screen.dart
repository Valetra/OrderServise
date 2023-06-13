import 'package:admin_panel/services/screen_arguments.dart';

import 'package:flutter/material.dart';

class SuppliesScreen extends StatefulWidget {
  const SuppliesScreen({super.key});

  static const routeName = '/supplies';
  @override
  State<SuppliesScreen> createState() => _SuppliesScreenState();
}

class _SuppliesScreenState extends State<SuppliesScreen> {
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
