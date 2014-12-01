/*exported ENUMS*/
'use strict';

var ENUMS = (function () {
    return {
        AccountStatus: {
            Closed: 0,
            Open: 1,
            Locked: 2
        },
        Product: {
            SportsBook: 0,
            VirtualSports: 5
        },
        TicketStatus: {
            Running: 0,
            Waiting: 1,
            Reject: 101,
            Void: 102,
            Refund: 103,
            Draw: 111,
            Won: 112,
            Lose: 113,
            Completed: 131,
            Approved: 132,
            Balance: 241,
            Sum: 142,
            Betting: "101,102,103,111,112,113",
            Outstanding: "0,1"
        }
    };
} ());