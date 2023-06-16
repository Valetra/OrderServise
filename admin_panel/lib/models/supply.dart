import 'dart:convert';
import 'package:flutter_guid/flutter_guid.dart';

List<Supply> supplyListFromJson(String str) =>
    List<Supply>.from(json.decode(str).map((x) => Supply.fromJson(x)));

String supplyListToJson(List<Supply> data) =>
    json.encode(List<dynamic>.from(data.map((x) => x.toJson())));

Supply supplyFromJson(String str) => Supply.fromJson(json.decode(str));

String orderToJson(Supply data) => json.encode(data.toJson());

class Supply {
  Guid? id;
  String name;
  dynamic count;
  int price;
  String cookingTime;
  Guid categoryId;

  Supply({
    this.id,
    required this.name,
    this.count,
    required this.price,
    required this.cookingTime,
    required this.categoryId,
  });

  Supply copy({
    Guid? id,
    String? name,
    int? count,
    int? price,
    String? cookingTime,
    Guid? categoryId,
  }) =>
      Supply(
        id: id ?? this.id,
        name: name ?? this.name,
        count: count ?? this.count,
        price: price ?? this.price,
        cookingTime: cookingTime ?? this.cookingTime,
        categoryId: categoryId ?? this.categoryId,
      );

  @override
  bool operator ==(Object other) =>
      identical(this, other) ||
      other is Supply &&
          runtimeType == other.runtimeType &&
          id == other.id &&
          name == other.name &&
          count == other.count &&
          price == other.price &&
          cookingTime == other.cookingTime &&
          categoryId == other.categoryId;

  @override
  int get hashCode =>
      id.hashCode ^
      name.hashCode ^
      count.hashCode ^
      price.hashCode ^
      cookingTime.hashCode ^
      categoryId.hashCode;

  factory Supply.fromJson(Map<String, dynamic> json) => Supply(
        id: Guid(json["id"]),
        name: json["name"],
        count: json["count"],
        price: json["price"],
        cookingTime: json["cookingTime"],
        categoryId: Guid(json["categoryId"]),
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
