/// <reference path="../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../libs/angular.js" />
/// <reference path="../../../../Views/Home/Index.cshtml" />


var fanex = fanex || {};

fanex.sportsbookSetting = angular.module('fanex.sportsbookSetting', ['fanex.angular']);

fanex.sportsbookSetting.FormType = {
    Add: 0,
    Edit: 1,
    ReadOnly: 2,
    EditIndirectMember: 3,
    EditMultiple: 4
};

fanex.sportsbookSetting.Role = {
    Licensee: 4,
    AgentLevel1: 3,
    Agent: 2,
    Member: 1
};

fanex.sportsbookSetting.filter('numberOnly', function (input) {
    input += 0;
    return input;
});
