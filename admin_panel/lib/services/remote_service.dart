import 'package:admin_panel/Models/order.dart';
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

    var uri = Uri.parse('http://localhost:5132/order');
    var response = await client.put(uri, body: order);

    if (response.statusCode == 200) {
      var json = response.body;

      return orderFromJson(json);
    }
    throw Exception("Nothing to return.");
  }
}
