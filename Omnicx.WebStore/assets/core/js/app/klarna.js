//Start ===============================================Added methods for Klarna Implementation===================================================
var createOrder = 'Klarna/CreateOrder';

// This is where you start calling Klarna's JS SDK functions
function KlarnaPaymentInit(token) {
    Klarna.Payments.init({
        client_token: token
    });
    Klarna.Payments.load({
        container: '#klarna_container',
        payment_method_category: 'pay_later'
    }, function (res) {
    })
}

function KlarnaAuthorize(checkoutModel, orderResp) {
    var orderLines = [];
    var orderNo = orderResp.refOrderId.split("-")[0];
    var paymentId = orderResp.refOrderId.split("-")[1];
    checkoutModel.basket.lineItems.forEach(function (item) {
        item;
        var line = {
            name: item.name,
            reference: item.stockCode,
            quantity: item.qty,
            unit_price: Math.round(item.price.raw.withTax * 100),
            total_amount: Math.round(item.price.raw.withTax * 100),
            type: "physical",
            quantity_unit: "pcs",
            tax_rate: item.price.raw.tax,
            total_discount_amount: 0,
            total_tax_amount: Math.round((item.price.raw.withTax * 100) / ((item.price.raw.withTax * 100) + item.price.raw.tax))
        };
        orderLines.push(line);
    });

    var shippingLine = {
        name: "Shipping",
        reference: "Shipping",
        quantity: 1,
        unit_price: Math.round(checkoutModel.basket.shippingCharge.raw.withTax * 100),
        total_amount: Math.round(checkoutModel.basket.shippingCharge.raw.withTax * 100),
        type: "shipping_fee",
        tax_rate: 0,
        total_discount_amount: 0,
        total_tax_amount: 0
    };
    orderLines.push(shippingLine);

    var discountLine = {
        name: "Discount",
        reference: "Discount",
        quantity: 1,
        unit_price: -Math.round(checkoutModel.basket.discount.raw.withTax * 100),
        total_amount: -Math.round(checkoutModel.basket.discount.raw.withTax * 100),
        type: "discount",
        tax_rate: 0,
        total_discount_amount: 0,
        total_tax_amount: 0
    };
    orderLines.push(discountLine);
    Klarna.Payments.authorize({
        payment_method_category: "pay_later"
        }, {
            purchase_country: checkoutModel.billingAddress.countryCode,
            purchase_currency: checkoutModel.currencyCode,
            locale: checkoutModel.lanuguageCode,
            billing_address: {
                given_name: checkoutModel.billingAddress.firstName,
                family_name: checkoutModel.billingAddress.lastName,
                email: checkoutModel.email,
                title: checkoutModel.billingAddress.title,
                street_address: checkoutModel.billingAddress.address1,
                street_address2: checkoutModel.billingAddress.address2,
                postal_code: checkoutModel.billingAddress.postCode,
                city: checkoutModel.billingAddress.city,
                region: checkoutModel.billingAddress.state,
                phone: checkoutModel.billingAddress.phoneNo,
                country: checkoutModel.billingAddress.countryCode,
            },
            shipping_address: {
                given_name: checkoutModel.shippingAddress.firstName,
                family_name: checkoutModel.shippingAddress.lastName,
                email: checkoutModel.email,
                title: checkoutModel.shippingAddress.title,
                street_address: checkoutModel.shippingAddress.address1,
                street_address2: checkoutModel.shippingAddress.address2,
                postal_code: checkoutModel.shippingAddress.postCode,
                city: checkoutModel.shippingAddress.city,
                region: checkoutModel.shippingAddress.state,
                phone: checkoutModel.shippingAddress.phoneNo,
                country: checkoutModel.shippingAddress.countryCode,
            },
            order_amount: Math.round(checkoutModel.basket.grandTotal.raw.withTax * 100),
            order_tax_amount: 0,
            order_lines: orderLines
        }, function (res) {
            if (res.authorization_token) {
                $.ajax({
                    type: "POST",
                    url: baseUrl + createOrder,
                    data: { "id": res.authorization_token, "processPaymentRequest": checkoutModel, "orderId": orderNo, "paymentId": paymentId },
                    success: klarna_order_success
                });
            }
        });
    function klarna_order_success(data) {
        window.location.href = data.notificationUrl + "?status=" + data.response.isValid + "&recordId=" + data.response.recordId;
    }
}
