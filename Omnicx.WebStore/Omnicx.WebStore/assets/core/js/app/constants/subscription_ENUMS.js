(function () {
    window.app.constant('SUBSCRIPTION_ENUMS', {
        'SubscriptionOrderTriggerType': {
            'FixedDay': 1,
            'Rolling': 2,
            'UserDefined': 3
        },
        'SubscriptionTermType': {
            'Fixed': 1,
            'UserDefined':2
        },
        'SubscriptionPlanType':{

            'Simple':"Simple",
            'FixedBundle':"FixedBundle",
            'DynamicBundle':"DynamicBundle"

        },
        'UserPricingType':{
            'None':"NONE",
            'OneTime' : "OneTime",
            'Recurring': "Recurring"
        }
    }
    );
}());