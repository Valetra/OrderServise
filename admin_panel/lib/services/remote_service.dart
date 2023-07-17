import 'package:admin_panel/Models/order.dart';
import 'package:admin_panel/models/category.dart';
import 'package:admin_panel/models/supply.dart';
import 'package:flutter_guid/flutter_guid.dart';
import 'package:admin_panel/models/responceObjects/responseOrderObject.dart';

import 'package:http/http.dart' as http;

class RemotesService {
  String apiUri = 'http://localhost:5132'; // https://localhost:7096

  Future<Category> updateCategory(Category category) async {
    var client = http.Client();

    String jsonRequest = '{"id": "${category.id}", "name": "${category.name}"}';

    var uri = Uri.parse('$apiUri/category');
    var response = await client.put(uri,
        headers: {'Content-Type': 'application/json'}, body: jsonRequest);

    if (response.statusCode == 200) {
      var json = response.body;

      return categoryFromJson(json);
    } else {
      throw Exception("Name repeat exception");
    }
  }

  Future createCategory(Category category) async {
    var client = http.Client();

    String jsonRequest = '{"name": "${category.name}"}';

    var uri = Uri.parse('$apiUri/category');
    var response = await client.post(uri,
        headers: {'Content-Type': 'application/json'}, body: jsonRequest);

    if (response.statusCode != 201) {
      throw Exception("Name repeat exception");
    }
  }

  Future deleteCategory(Guid categoryId) async {
    var client = http.Client();

    var uri = Uri.parse('$apiUri/category/$categoryId');

    var response =
        await client.delete(uri, headers: {'Content-Type': 'application/json'});

    if (response.statusCode != 204) {
      throw Exception("Nothing to return.");
    }
  }

  Future<List<Supply>> getOrderSupplies(Guid id) async {
    var client = http.Client();

    var uri = Uri.parse('$apiUri/order/getOrderWithSupplies/$id');
    var response = await client.get(uri);

    if (response.statusCode == 200) {
      var json = response.body;

      var responseOrderObject = responseOrderObjectFromJson(json);

      return responseOrderObject.supplies;
    }
    throw Exception("Nothing to return.");
  }

  Future createSupply(Supply supply) async {
    var client = http.Client();

    String jsonRequest =
        '{"name": "${supply.name}", "count" : ${supply.count}, "price" : ${supply.price}, "cookingTime": "${supply.cookingTime}", "categoryId": "${supply.categoryId}"}';

    var uri = Uri.parse('$apiUri/supply');
    var response = await client.post(uri,
        headers: {'Content-Type': 'application/json'}, body: jsonRequest);

    if (response.statusCode != 201) {
      throw Exception("Name repeat exception");
    }
  }

  Future deleteSupply(Guid supplyId) async {
    var client = http.Client();

    var uri = Uri.parse('$apiUri/supply/$supplyId');

    var response =
        await client.delete(uri, headers: {'Content-Type': 'application/json'});

    if (response.statusCode != 204) {
      throw Exception("Nothing to return.");
    }
  }

  Future<List<Order>> getOrderList() async {
    var client = http.Client();

    var uri = Uri.parse('$apiUri/order');
    var response = await client.get(uri);

    if (response.statusCode == 200) {
      var json = response.body;

      return orderListFromJson(json);
    }
    throw Exception("Nothing to return.");
  }

  Future<Order> updateOrder(Order order) async {
    var client = http.Client();

    String jsonRequest =
        '{"id": "${order.id}", "status": "${order.status}", "payed" : ${order.payed}}';

    var uri = Uri.parse('$apiUri/order');
    var response = await client.put(uri,
        headers: {'Content-Type': 'application/json'}, body: jsonRequest);

    if (response.statusCode == 200) {
      var json = response.body;

      return orderFromJson(json);
    }
    throw Exception("${response.reasonPhrase}");
  }

  Future<List<Supply>> getSupplyList() async {
    var client = http.Client();

    var uri = Uri.parse('$apiUri/supply');
    var response = await client.get(uri);

    if (response.statusCode == 200) {
      var json = response.body;

      return supplyListFromJson(json);
    }
    throw Exception("Nothing to return.");
  }

  Future<Supply> updateSupply(Supply supply) async {
    var client = http.Client();

    String jsonRequest =
        '{"id": "${supply.id}", "name": "${supply.name}", "count" : ${supply.count}, "price" : ${supply.price}, "cookingTime": "${supply.cookingTime}", "categoryId": "${supply.categoryId}"}';

    var uri = Uri.parse('$apiUri/supply');
    var response = await client.put(uri,
        headers: {'Content-Type': 'application/json'}, body: jsonRequest);

    if (response.statusCode == 200) {
      var json = response.body;

      return supplyFromJson(json);
    } else {
      throw Exception("Name repeat exception");
    }
  }

  Future<List<Category>> getCategoryList() async {
    var client = http.Client();

    var uri = Uri.parse('$apiUri/category');
    var response = await client.get(uri);

    if (response.statusCode == 200) {
      var json = response.body;

      return categoryListFromJson(json);
    }
    throw Exception("Nothing to return.");
  }
}
