import 'package:flutter_guid/flutter_guid.dart';
import 'dart:convert';

List<Category> categoryListFromJson(String str) =>
    List<Category>.from(json.decode(str).map((x) => Category.fromJson(x)));

String categoryListToJson(List<Category> data) =>
    json.encode(List<dynamic>.from(data.map((x) => x.toJson())));

class Category {
  String name;
  Guid id;

  Category({
    required this.name,
    required this.id,
  });

  factory Category.fromJson(Map<String, dynamic> json) => Category(
        name: json["name"],
        id: Guid(json["id"]),
      );

  Map<String, dynamic> toJson() => {
        "name": name,
        "id": id,
      };
}
