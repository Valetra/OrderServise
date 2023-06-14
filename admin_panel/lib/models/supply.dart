import 'dart:convert';
import 'package:flutter_guid/flutter_guid.dart';

List<Supply> supplyListFromJson(String str) =>
    List<Supply>.from(json.decode(str).map((x) => Supply.fromJson(x)));

String supplyListToJson(List<Supply> data) =>
    json.encode(List<dynamic>.from(data.map((x) => x.toJson())));

class Supply {
  Guid id;
  String name;
  int count;
  int price;
  String cookingTime;
  String categoryId;

  Supply({
    required this.id,
    required this.name,
    required this.count,
    required this.price,
    required this.cookingTime,
    required this.categoryId,
  });

  factory Supply.fromJson(Map<String, dynamic> json) => Supply(
        id: Guid(json["id"]),
        name: json["name"],
        count: json["count"],
        price: json["price"],
        cookingTime: json["cookingTime"],
        categoryId: json["categoryId"],
      );

  Map<String, dynamic> toJson() => {
        "id": id,
        "name": name,
        "count": count,
        "price": price,
        "cookingTime": cookingTime,
        "categoryId": categoryId,
      };
}
