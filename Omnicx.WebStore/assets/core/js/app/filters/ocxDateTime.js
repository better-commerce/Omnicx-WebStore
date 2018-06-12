window.app.filter('ocxDateTime', function ($filter) {
    var angularDateFilter = $filter('date');
    return function (theDate) {
        return angularDateFilter(theDate, 'dd-MMM-yy @ HH:mm');
    }
});
window.app.filter('ocxTime', function ($filter) {
    var angularDateFilter = $filter('date');
    return function (theDate) {
        return angularDateFilter(theDate, 'HH:mm:ss');
    }
});