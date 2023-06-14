import 'package:admin_panel/Models/order.dart';
import 'package:admin_panel/models/category.dart';
import 'package:admin_panel/models/supply.dart';
import 'package:http/http.dart' as http;

class RemotesService {
  Future<List<Order>> getOrderList() async {
    var client = http.Client();

    var uri = Uri.parse('http://localhost:5132/order');
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

    var uri = Uri.parse('http://localhost:5132/order');
    var response = await client.put(uri,
        headers: {'Content-Type': 'application/json'}, body: jsonRequest);

    if (response.statusCode == 200) {
      var json = response.body;

      return orderFromJson(json);
    }
    throw Exception("Nothing to return.");
  }

  Future<List<Supply>> getSupplyList() async {
    var client = http.Client();

    var uri = Uri.parse('http://localhost:5132/supply');
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

    var uri = Uri.parse('http://localhost:5132/supply');
    var response = await client.put(uri,
        headers: {'Content-Type': 'application/json'}, body: jsonRequest);

    if (response.statusCode == 200) {
      var json = response.body;

      return supplyFromJson(json);
    }
    throw Exception(response.reasonPhrase);
  }

  Future<List<Category>> getCategoryList() async {
    var client = http.Client();

    var uri = Uri.parse('http://localhost:5132/category');
    var response = await client.get(uri);

    if (response.statusCode == 200) {
      var json = response.body;

      return categoryListFromJson(json);
    }
    throw Exception("Nothing to return.");
  }
}
