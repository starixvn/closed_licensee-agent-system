/// <reference path="../../../../libs/angular.js" />
/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/*global commissionSetting, settingContext */

'use strict';

if (fanex.customerSetting) {
    fanex.customerSetting.numberGameCommissionService = {
        getCommissionSetting: function(commissionSettingModel, commissionSettingViewModel, targetRoleId) {
            var commissionSetting = commissionSettingModel[4];

            if (settingContext.FormId === fanex.customerSetting.FormType.Add) {
                if (targetRoleId !== 1) {
                    commissionSetting.DiscountA = commissionSettingViewModel[3].discount;
                } else {
                    commissionSetting.Discount = commissionSettingViewModel.line5.memberCommission;
                }
            } else {
                if (targetRoleId !== 1) {
                    if (commissionSettingViewModel[3].discount !== commissionSettingViewModel[3].oldDiscount) {
                        commissionSetting.DiscountA = commissionSettingViewModel[3].discount;
                    }
                } else {
                    if (commissionSettingViewModel.line5.memberCommission !== commissionSettingViewModel.line5.oldCommission) {
                        commissionSetting.Discount = commissionSettingViewModel.line5.memberCommission;
                    }
                }
            }
            return [commissionSetting];
        }
    };
}