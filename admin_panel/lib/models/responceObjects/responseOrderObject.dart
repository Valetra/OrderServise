import 'dart:convert';
import 'package:admin_panel/models/supply.dart';

ResponseOrderObject responseOrderObjectFromJson(String str) =>
    ResponseOrderObject.fromJson(json.decode(str));

String responseOrderObjectToJson(ResponseOrderObject data) =>
    json.encode(data.toJson());

class ResponseOrderObject {
  String status;
  bool payed;
  DateTime createDateTime;
  int number;
  List<Supply> supplies;

  ResponseOrderObject({
    required this.status,
    required this.payed,
    required this.createDateTime,
    required this.number,
    required this.supplies,
  });

  factory ResponseOrderObject.fromJson(Map<String, dynamic> json) =>
      ResponseOrderObject(
        status: json["status"],
        payed: json["payed"],
        createDateTime: DateTime.parse(json["createDateTime"]),
        number: json["number"],
        supplies:
            List<Supply>.from(json["supplies"].map((x) => Supply.fromJson(x))),
      );

  Map<String, dynamic> toJson() => {
        "status": status,
        "payed": payed,
        "createDateTime": createDateTime.toIso8601String(),
        "number": number,
        "supplies": List<dynamic>.from(supplies.map((x) => x.toJson())),
      };
}
