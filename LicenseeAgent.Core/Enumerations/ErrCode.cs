// --------------------------------------------------------------------------
// <copyright file="ErrCode.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Enumerations
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Update later.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed.")]
    public enum ErrCode : int
    {
        /// <summary>
        /// Successfully.
        /// </summary>
        None = 0,

        /// <summary>
        /// Min Bet should be less than or equal to Max Bet({@BetLimitSportType})
        /// </summary>
        Msg_1003 = 1003,

        /// <summary>
        /// Min Bet must be less than or equal to Max Per Match({@BetLimitSportType}).
        /// </summary>
        Msg_1004 = 1004,

        /// <summary>
        /// Max Bet should be less than or equal to Max Per Match({@BetLimitSportType})
        /// </summary>
        Msg_1005 = 1005,

        /// <summary>
        /// Min Bet must be equal to or greater than Parent Agent Min Bet({@BetLimitSportType})
        /// </summary>
        Msg_1006 = 1006,

        /// <summary>
        /// Max Bet must be equal to or less than Parent Agent Max Bet({@BetLimitSportType})
        /// </summary>
        Msg_1007 = 1007,

        /// <summary>
        /// Max Per Match must be equal to or less than Parent Agent Max Per Match({@BetLimitSportType})
        /// </summary>
        Msg_1008 = 1008,

        /// <summary>
        /// % PT must be equal or greater than zero ( {@PTSportType} - {@PTBetType}).
        /// </summary>
        Msg_1010 = 1010,

        /// <summary>
        /// % Min given PT must be equal to or less than % Max given PT ( {@PTSportType} - {@PTBetType}).
        /// </summary>
        Msg_1011 = 1011,

        /// <summary>
        /// % PT must be equal or less than Max PT of Licensee( {@PTSportType} - {@PTBetType}).
        /// </summary>
        Msg_1012 = 1012,

        /// <summary>
        /// %Min is Invalid Value, You must in {@MinPTOutput} and {@MaxPTOutput}  ( {@PTSportType} - {@PTBetType}).
        /// </summary>
        Msg_1013 = 1013,

        /// <summary>
        /// %Max is Invalid Value, You must in  {@MinPTOutput} and {@MaxPTOutput}  ( {@PTSportType} - {@PTBetType}).
        /// </summary>
        Msg_1014 = 1014,

        /// <summary>
        ///   %Preset is Invalid Value, You must in  {@MinPTOutput} and {@MaxPTOutput}  ( {@PTSportType} - {@PTBetType}).
        /// </summary>
        Msg_1015 = 1015,

        /// <summary>
        /// Given credit must be between 0 to {@MaxCreditOutput}.
        /// </summary>
        Msg_1016 = 1016,

        /// <summary>
        /// %PT is Invalid Value, You must in {@MinPTOutput} and {@MaxPTOutput}  ( {@PTSportType} - {@PTBetType}).
        /// </summary>
        Msg_1021 = 1021,

        /// <summary>
        /// Fail to update agent's commission.
        /// </summary>
        Msg_1024 = 1024,

        /// <summary>
        /// Fail to update agent's bet setting.
        /// </summary>
        Msg_1025 = 1025,

        /// <summary>
        /// Fail to update agent's position taking.
        /// </summary>
        Msg_1026 = 1026,

        /// <summary>
        /// Fail to update member's commission.
        /// </summary>
        Msg_1027 = 1027,

        /// <summary>
        /// Fail to update member's betlimit.
        /// </summary>
        Msg_1028 = 1028,

        /// <summary>
        /// Fail to update member's position taking.
        /// </summary>
        Msg_1029 = 1029,

        /// <summary>
        /// You don't have permission to update this account settings.
        /// </summary>
        Msg_1030 = 1030,

        /// <summary>
        /// "... Another session has been logged in ...
        /// Your session will be terminated!!!
        /// If that is not you, please contact your upline!!!"
        /// </summary>
        Msg_1037 = 1037,

        /// <summary>
        /// The licensee was changed by admin, this session must be closed.
        /// </summary>
        Msg_1038 = 1038
    }
}