import 'package:flutter_guid/flutter_guid.dart';
import 'dart:convert';

List<Order> orderListFromJson(String str) =>
    List<Order>.from(json.decode(str).map((x) => Order.fromJson(x)));

String orderListToJson(List<Order> data) =>
    json.encode(List<dynamic>.from(data.map((x) => x.toJson())));

Order orderFromJson(String str) => Order.fromJson(json.decode(str));

String orderToJson(Order data) => json.encode(data.toJson());

class Order {
  String status;
  bool payed;
  DateTime createDateTime;
  int number;
  dynamic orderSupply;
  Guid id;

  Order({
    required this.status,
    required this.payed,
    required this.createDateTime,
    required this.number,
    this.orderSupply,
    required this.id,
  });

  factory Order.fromJson(Map<String, dynamic> json) => Order(
        status: json["status"],
        payed: json["payed"],
        createDateTime: DateTime.parse(json["createDateTime"]),
        number: json["number"],
        orderSupply: json["orderSupply"],
        id: Guid(json["id"]),
      );

  Map<String, dynamic> toJson() => {
        "status": status,
        "payed": payed,
        "createDateTime": createDateTime.toIso8601String(),
        "number": number,
        "orderSupply": orderSupply,
        "id": id,
      };
}
