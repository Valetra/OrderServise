import 'package:admin_panel/Models/order.dart';
import 'package:http/http.dart' as http;

class RemotesService {
  Future<List<Order>?> getOrders() async {
    var client = http.Client();

    var uri = Uri.parse('http://localhost:5132/order');
    var response = await client.get(uri);

    if (response.statusCode == 200) {
      var json = response.body;

      return orderFromJson(json);
    }
    return null;
  }
}
