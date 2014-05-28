/*global settingContext */

if (fanex.customerSetting) {
    fanex.customerSetting.numberGameCommissionService = {
        getCommissionSetting: function (commissionSettingModel, commissionSettingViewModel, targetRoleId) {
            'use strict';
            var commissionSetting = commissionSettingModel[4];
            if (settingContext.FormId === fanex.customerSetting.FormType.Add) {
                if (targetRoleId !== 1) {
                    commissionSetting.DiscountA = commissionSettingViewModel[7].discount;
                } else {
                    commissionSetting.Discount = commissionSettingViewModel.line5.memberCommission;
                }
            } else {
                if (targetRoleId !== 1) {
                    if (commissionSettingViewModel[7].discount !== commissionSettingViewModel[7].oldDiscount) {
                        commissionSetting.DiscountA = commissionSettingViewModel[7].discount;
                    }
                    else {
                        commissionSetting = null;
                    }
                } else {
                    if (commissionSettingViewModel.line5.memberCommission !== commissionSettingViewModel.line5.oldCommission) {
                        commissionSetting.Discount = commissionSettingViewModel.line5.memberCommission;
                    }
                    else {
                        commissionSetting = null;
                    }
                }
            }
            return {
                CommissionItems: commissionSetting === null ? [] : [commissionSetting]
            };
        }
    };
}