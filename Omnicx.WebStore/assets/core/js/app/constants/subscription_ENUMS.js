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
        }, 
    }
    );
    window.app.constant('SUBSCRIPTION_CONSTANTS', {
        'SubscriptionPlanPricingType': {
            'Flat': 1,
            'Term': 2,
            'PerUnit': 3
        },
        'UserPricingType': {
            'None': "None",
            'OneTime': "OneTime",
            'Recurring': "Recurring"
        },
        'SubscriptionPlanType': {
            'Simple': "Simple",
            'FixedBundle': "FixedBundle",
            'DynamicBundle': "DynamicBundle"
        },
    });
}());