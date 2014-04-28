/// <reference path="../../../../libs/angular.js" />
/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/*global commissionSetting, settingContext */

'use strict';

if (fanex.customerSetting) {
    //// Helper

    fanex.customerSetting.commissionSettingService = {
        config: {
            maxInput: 5
        },
        showInvalidMessage: function(element, message) {
            fanex.showHint(element, message);
        },
        isValidData: function(commissionSettings) {
            if (commissionSettings instanceof Array) {
                for (var i = 0; i < commissionSettings.length; i++) {
                    if (!commissionSettings[i].actions.isValid()) {
                        return false;
                    }
                }
            } else {
                return commissionSettings.actions.isValid();
            }

            return true;
        },
        getCommissionSettingViewModel: function(commissionSettingModel, language, targetRoleId, haveNumberGame) {
            var commissionSettingViewModel;

            if (targetRoleId !== 1) {
                commissionSettingViewModel = [];

                // GroupA
                commissionSettingViewModel.push({
                    minDiscount: 0,
                    discount: commissionSettingModel[0].DiscountA,
                    oldDiscount: commissionSettingModel[0].DiscountA,
                    maxDiscount: commissionSettingModel[0].Upline.DiscountA,
                    name: language['commissionSetting_GroupA']
                });

                // GroupC
                commissionSettingViewModel.push({
                    minDiscount: 0,
                    discount: commissionSettingModel[0].DiscountC,
                    oldDiscount: commissionSettingModel[0].DiscountC,
                    maxDiscount: commissionSettingModel[0].Upline.DiscountC,
                    name: language['commissionSetting_GroupC']
                });

                // 1x2 Comm
                commissionSettingViewModel.push({
                    minDiscount: 0,
                    discount: commissionSettingModel[1].DiscountA,
                    oldDiscount: commissionSettingModel[1].DiscountA,
                    maxDiscount: commissionSettingModel[1].Upline.DiscountA,
                    name: language['commissionSetting_1x2Comm']
                });

                // GroupB
                commissionSettingViewModel.push({
                    minDiscount: 0,
                    discount: commissionSettingModel[0].DiscountB,
                    oldDiscount: commissionSettingModel[0].DiscountB,
                    maxDiscount: commissionSettingModel[0].Upline.DiscountB,
                    name: language['commissionSetting_GroupB']
                });

                // GroupD
                commissionSettingViewModel.push({
                    minDiscount: 0,
                    discount: commissionSettingModel[0].DiscountD,
                    oldDiscount: commissionSettingModel[0].DiscountD,
                    maxDiscount: commissionSettingModel[0].Upline.DiscountD,
                    name: language['commissionSetting_GroupD']
                });

                // Others Comm
                commissionSettingViewModel.push({
                    minDiscount: 0,
                    discount: commissionSettingModel[2].DiscountA,
                    oldDiscount: commissionSettingModel[2].DiscountA,
                    maxDiscount: commissionSettingModel[2].Upline.DiscountA,
                    name: language['commissionSetting_OthersComm']
                });

                // HR Fixed Odds
                commissionSettingViewModel.push({
                    minDiscount: 0,
                    discount: commissionSettingModel[3].DiscountA,
                    oldDiscount: commissionSettingModel[3].DiscountA,
                    maxDiscount: commissionSettingModel[3].Upline.DiscountA,
                    name: language['commissionSetting_HrFixedOdds']
                });

                // Number Game
                if (haveNumberGame) {
                    commissionSettingViewModel.push({
                        minDiscount: 0,
                        discount: commissionSettingModel[4].DiscountA,
                        oldDiscount: commissionSettingModel[4].DiscountA,
                        maxDiscount: commissionSettingModel[4].Upline.DiscountA,
                        name: language['commissionSetting_NumberGame']
                    });
                }

                return commissionSettingViewModel;
            } else {
                // Insert
                if (settingContext.FormId === fanex.customerSetting.FormType.Add) {
                    commissionSettingViewModel = {
                        selectedGroup: 'A',
                        group: [
                            {
                                name: 'A',
                                agentCommission: commissionSettingModel[0].Upline.DiscountA,
                                memberCommission: commissionSettingModel[0].DiscountA
                            },
                            {
                                name: 'B',
                                agentCommission: commissionSettingModel[0].Upline.DiscountB,
                                memberCommission: commissionSettingModel[0].DiscountB
                            },
                            {
                                name: 'C',
                                agentCommission: commissionSettingModel[0].Upline.DiscountC,
                                memberCommission: commissionSettingModel[0].DiscountC
                            },
                            {
                                name: 'D',
                                agentCommission: commissionSettingModel[0].Upline.DiscountD,
                                memberCommission: commissionSettingModel[0].DiscountD
                            }
                        ],
                        // GroupA, GroupB, GroupC, GroupD
                        line1: {
                            name: language['commissionSetting_HDP_OU_OE_Comm'],
                            agentCommission: commissionSettingModel[0].Upline.DiscountA,
                            memberCommission: commissionSettingModel[0].DiscountA
                        },
                        // 1x2 Comm
                        line2: {
                            name: language['commissionSetting_1x2Comm'],
                            agentCommission: commissionSettingModel[1].Upline.DiscountA,
                            memberCommission: commissionSettingModel[1].DiscountA
                        },
                        // Others Comm
                        line3: {
                            name: language['commissionSetting_OthersComm'],
                            agentCommission: commissionSettingModel[2].Upline.DiscountA,
                            memberCommission: commissionSettingModel[2].DiscountA
                        },
                        // HR Fixed Odds Comm
                        line4: {
                            name: language['commissionSetting_HrFixedOddsComm'],
                            agentCommission: commissionSettingModel[3].Upline.DiscountA,
                            memberCommission: commissionSettingModel[3].Discount
                        }
                    };

                    // Number Game
                    if (haveNumberGame) {
                        commissionSettingViewModel.line5 = {
                            name: language['commissionSetting_NumberGame'],
                            agentCommission: commissionSettingModel[4].Upline.DiscountA,
                            memberCommission: commissionSettingModel[4].Discount
                        };
                    }

                    return commissionSettingViewModel;
                }
                // Edit
                else {
                    commissionSettingViewModel = {
                        selectedGroup: commissionSetting.UserGroup,
                        oldSelectedGroup: commissionSetting.UserGroup,
                        group: [
                            {
                                name: 'A',
                                agentCommission: commissionSettingModel[0].Upline.DiscountA,
                                memberCommission: (commissionSetting.UserGroup === 'A'
                                    ? commissionSettingModel[0].Discount
                                    : commissionSettingModel[0].Upline.DiscountA)
                            },
                            {
                                name: 'B',
                                agentCommission: commissionSettingModel[0].Upline.DiscountB,
                                memberCommission: (commissionSetting.UserGroup === 'B'
                                    ? commissionSettingModel[0].Discount
                                    : commissionSettingModel[0].Upline.DiscountB)
                            },
                            {
                                name: 'C',
                                agentCommission: commissionSettingModel[0].Upline.DiscountC,
                                memberCommission: (commissionSetting.UserGroup === 'C'
                                    ? commissionSettingModel[0].Discount
                                    : commissionSettingModel[0].Upline.DiscountC)
                            },
                            {
                                name: 'D',
                                agentCommission: commissionSettingModel[0].Upline.DiscountD,
                                memberCommission: (commissionSetting.UserGroup === 'D'
                                    ? commissionSettingModel[0].Discount
                                    : commissionSettingModel[0].Upline.DiscountD)
                            }
                        ],
                        // GroupA, GroupB, GroupC, GroupD
                        line1: {
                            name: language['commissionSetting_HDP_OU_OE_Comm'],
                            agentCommission: 0,
                            memberCommission: 0,
                            oldCommission: 0
                        },
                        // 1x2 Comm
                        line2: {
                            name: language['commissionSetting_1x2Comm'],
                            agentCommission: commissionSettingModel[1].Upline.DiscountA,
                            memberCommission: commissionSettingModel[1].Discount,
                            oldCommission: commissionSettingModel[1].Discount
                        },
                        // Others Comm
                        line3: {
                            name: language['commissionSetting_OthersComm'],
                            agentCommission: commissionSettingModel[2].Upline.DiscountA,
                            memberCommission: commissionSettingModel[2].Discount,
                            oldCommission: commissionSettingModel[2].Discount
                        },
                        // HR Fixed Odds Comm
                        line4: {
                            name: language['commissionSetting_HrFixedOddsComm'],
                            agentCommission: commissionSettingModel[3].Upline.DiscountA,
                            memberCommission: commissionSettingModel[3].Discount,
                            oldCommission: commissionSettingModel[3].Discount
                        }
                    };

                    // Number Game
                    if (haveNumberGame) {
                        commissionSettingViewModel.line5 = {
                            name: language['commissionSetting_NumberGame'],
                            agentCommission: commissionSettingModel[4].Upline.DiscountA,
                            memberCommission: commissionSettingModel[4].Discount,
                            oldCommission: commissionSettingModel[4].Discount
                        };
                    }

                    return this.changeViewModel(commissionSettingViewModel);
                }
            }
        },
        getCommissionSettingModel: function(commissionSettingModel, commissionSettingViewModel, targetRoleId) {
            // Insert
            if (settingContext.FormId === fanex.customerSetting.FormType.Add) {
                if (targetRoleId !== 1) {
                    // GroupA
                    commissionSettingModel[0].DiscountA = commissionSettingViewModel[0].discount;
                    // GroupB
                    commissionSettingModel[0].DiscountB = commissionSettingViewModel[3].discount;
                    // GroupC
                    commissionSettingModel[0].DiscountC = commissionSettingViewModel[1].discount;
                    // GroupD
                    commissionSettingModel[0].DiscountD = commissionSettingViewModel[4].discount;
                    // 1x2 Comm
                    commissionSettingModel[1].DiscountA = commissionSettingViewModel[2].discount;
                    // Others Comm
                    commissionSettingModel[2].DiscountA = commissionSettingViewModel[5].discount;
                    // HR Fixed Odds
                    commissionSettingModel[3].DiscountA = commissionSettingViewModel[6].discount;
                } else {
                    commissionSettingModel[0].Discount = commissionSettingViewModel.line1.memberCommission;
                    // 1x2 Comm
                    commissionSettingModel[1].Discount = commissionSettingViewModel.line2.memberCommission;
                    // Others Comm
                    commissionSettingModel[2].Discount = commissionSettingViewModel.line3.memberCommission;
                    // HR Fixed Odds
                    commissionSettingModel[3].Discount = commissionSettingViewModel.line4.memberCommission;
                }

                return commissionSettingModel;
            }
            // Edit
            else {
                var commissionSettings = [];

                if (targetRoleId !== 1) {
                    if (commissionSettingViewModel[0].discount !== commissionSettingViewModel[0].oldDiscount
                        || commissionSettingViewModel[3].discount !== commissionSettingViewModel[3].oldDiscount
                        || commissionSettingViewModel[1].discount !== commissionSettingViewModel[1].oldDiscount
                        || commissionSettingViewModel[4].discount !== commissionSettingViewModel[4].oldDiscount) {
                        commissionSettingModel[0].DiscountA = commissionSettingViewModel[0].discount;
                        commissionSettingModel[0].DiscountB = commissionSettingViewModel[3].discount;
                        commissionSettingModel[0].DiscountC = commissionSettingViewModel[1].discount;
                        commissionSettingModel[0].DiscountD = commissionSettingViewModel[4].discount;
                        commissionSettings.push(commissionSettingModel[0]);
                    }
                    // 1x2 Comm
                    if (commissionSettingViewModel[2].discount !== commissionSettingViewModel[2].oldDiscount) {
                        commissionSettingModel[1].DiscountA = commissionSettingViewModel[2].discount;
                        commissionSettings.push(commissionSettingModel[1]);
                    }
                    // Others Comm
                    if (commissionSettingViewModel[5].discount !== commissionSettingViewModel[5].oldDiscount) {
                        commissionSettingModel[2].DiscountA = commissionSettingViewModel[5].discount;
                        commissionSettings.push(commissionSettingModel[2]);
                    }
                    // HR Fixed Odds
                    if (commissionSettingViewModel[6].discount !== commissionSettingViewModel[6].oldDiscount) {
                        commissionSettingModel[3].DiscountA = commissionSettingViewModel[6].discount;
                        commissionSettings.push(commissionSettingModel[3]);
                    }
                } else {
                    if (commissionSettingViewModel.selectedGroup !== commissionSettingViewModel.oldSelectedGroup
                        || (commissionSettingViewModel.selectedGroup === commissionSettingViewModel.oldSelectedGroup &&
                            commissionSettingViewModel.line1.memberCommission !== commissionSettingViewModel.line1.oldCommission)) {
                        commissionSettingModel[0].Discount = commissionSettingViewModel.line1.memberCommission;
                        commissionSettings.push(commissionSettingModel[0]);
                    }
                    // 1x2 Comm
                    if (commissionSettingViewModel.line2.memberCommission !== commissionSettingViewModel.line2.oldCommission) {
                        commissionSettingModel[1].Discount = commissionSettingViewModel.line2.memberCommission;
                        commissionSettings.push(commissionSettingModel[1]);
                    }
                    // Others Comm
                    if (commissionSettingViewModel.line3.memberCommission !== commissionSettingViewModel.line3.oldCommission) {
                        commissionSettingModel[2].Discount = commissionSettingViewModel.line3.memberCommission;
                        commissionSettings.push(commissionSettingModel[2]);
                    }
                    // HR Fixed Odds
                    if (commissionSettingViewModel.line4.memberCommission !== commissionSettingViewModel.line4.oldCommission) {
                        commissionSettingModel[3].Discount = commissionSettingViewModel.line4.memberCommission;
                        commissionSettings.push(commissionSettingModel[3]);
                    }
                }

                return commissionSettings;
            }
        },
        changeViewModel: function(commissionSettingViewModel) {
            if (commissionSettingViewModel.selectedGroup === 'A') {
                commissionSettingViewModel.line1.agentCommission = commissionSettingViewModel.group[0].agentCommission;
                commissionSettingViewModel.line1.memberCommission = commissionSettingViewModel.group[0].memberCommission;
                commissionSettingViewModel.line1.oldCommission = commissionSettingViewModel.group[0].memberCommission;
            } else if (commissionSettingViewModel.selectedGroup === 'B') {
                commissionSettingViewModel.line1.agentCommission = commissionSettingViewModel.group[1].agentCommission;
                commissionSettingViewModel.line1.memberCommission = commissionSettingViewModel.group[1].memberCommission;
                commissionSettingViewModel.line1.oldCommission = commissionSettingViewModel.group[1].memberCommission;
            } else if (commissionSettingViewModel.selectedGroup === 'C') {
                commissionSettingViewModel.line1.agentCommission = commissionSettingViewModel.group[2].agentCommission;
                commissionSettingViewModel.line1.memberCommission = commissionSettingViewModel.group[2].memberCommission;
                commissionSettingViewModel.line1.oldCommission = commissionSettingViewModel.group[2].memberCommission;
            } else if (commissionSettingViewModel.selectedGroup === 'D') {
                commissionSettingViewModel.line1.agentCommission = commissionSettingViewModel.group[3].agentCommission;
                commissionSettingViewModel.line1.memberCommission = commissionSettingViewModel.group[3].memberCommission;
                commissionSettingViewModel.line1.oldCommission = commissionSettingViewModel.group[3].memberCommission;
            }

            return commissionSettingViewModel;
        },
        arrangeCommissionSettingModel: function(commissionSettingModel) {
            var result = [];
            for (var i = 0; i < commissionSettingModel.length; i++) {
                if (commissionSettingModel[i].BetTypeId === 1) {
                    result[0] = commissionSettingModel[i];
                } else if (commissionSettingModel[i].BetTypeId === 5) {
                    result[1] = commissionSettingModel[i];
                } else if (commissionSettingModel[i].BetTypeId === 4) {
                    result[2] = commissionSettingModel[i];
                } else if (commissionSettingModel[i].SportId === 154 && commissionSettingModel[i].BetTypeId === 20) {
                    result[3] = commissionSettingModel[i];
                } else if (commissionSettingModel[i].SportId === 161 && commissionSettingModel[i].BetTypeId === 81) {
                    result[4] = commissionSettingModel[i];
                }
            }

            return result;
        }
    };
}