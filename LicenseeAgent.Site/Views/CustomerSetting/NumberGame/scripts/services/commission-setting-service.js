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
                    commissionSetting.DiscountA = commissionSettingViewModel[4].discount;
                } else {
                    commissionSetting.Discount = commissionSettingViewModel.line6.memberCommission;
                }
            } else {
                if (targetRoleId !== 1) {
                    if (commissionSettingViewModel[4].discount !== commissionSettingViewModel[4].oldDiscount) {
                        commissionSetting.DiscountA = commissionSettingViewModel[4].discount;
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