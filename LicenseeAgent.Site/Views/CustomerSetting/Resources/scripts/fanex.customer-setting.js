/// <reference path="../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../libs/angular.js" />
/// <reference path="../../../../Views/Home/Index.cshtml" />

/*global fanex: true*/

'use strict';

fanex = fanex || {};

fanex.customerSetting = angular.module('fanex.customerSetting', ['fanex.angular']);

fanex.customerSetting.FormType = {
    Add: 0,
    Edit: 1,
    ViewIndirectAgent: 2,
    EditIndirectMember: 3,
    EditMultiple: 4
};

fanex.customerSetting.Role = {
    Licensee: 4,
    AgentLevel1: 3,
    Agent: 2,
    Member: 1
};

fanex.customerSetting.filter('numberOnly', function(input) {
    input += 0;
    return input;
});
