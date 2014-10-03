var core = core || {};
core.enums = (function () {
    'use strict';
    return {
        Status: {
            Closed: 0,
            Open: 1,
            Locked: 2
        },
        Product: {
            SportsBook: 0,
            VirtualSports: 5
        }
    };
} ());