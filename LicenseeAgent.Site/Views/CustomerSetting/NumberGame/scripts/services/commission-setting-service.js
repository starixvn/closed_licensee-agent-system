/// <reference path="../../../../libs/angular.js" />
/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/*global settingContext */

'use strict';

if (fanex.customerSetting) {
    fanex.customerSetting.numberGameCommissionService = {
        getCommissionSetting: function(commissionSettingModel, commissionSettingViewModel, targetRoleId) {
            var commissionSetting = commissionSettingModel[4];
            if (settingContext.FormId === fanex.customerSetting.FormType.Add) {
                if (targetRoleId !== 1) {
                    commissionSetting.Discount = commissionSettingViewModel[7].discount;
                } else {
                    commissionSetting.Discount = commissionSettingViewModel.line6.memberCommission;
                }
            } else {
                if (targetRoleId !== 1) {
                    if (commissionSettingViewModel[7].discount !== commissionSettingViewModel[7].oldDiscount) {
                        commissionSetting.Discount = commissionSettingViewModel[7].discount;
                    }
                } else {
                    if (commissionSettingViewModel.line6.memberCommission !== commissionSettingViewModel.line6.oldCommission) {
                        commissionSetting.Discount = commissionSettingViewModel.line6.memberCommission;
                    }
                }
            }
            return [commissionSetting];
        }
    };
}