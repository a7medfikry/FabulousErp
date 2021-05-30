var MemoryCache = function (options) {
    options = $.extend(true, {
        maxEntries: null
    }, options || {});

    var self = this;
    var cache = {};
    var entries = 0;

    var ageOf = function (entry) {
        return Math.round((new Date() - entry.setAt) / 1000);
    };

    var deleteEntryByKey = function (key) {
        delete cache[key];
        if (entries > 0) entries--;
    };

    var deleteOldestEntry = function () {
        var keyOfOldestItem, dateOfOldestItem;
        for (var key in cache) {
            if (!dateOfOldestItem || dateOfOldestItem > cache[key].setAt) {
                dateOfOldestItem = cache[key].setAt;
                keyOfOldestItem = key;
            }
        }

        if (keyOfOldestItem) {
            deleteEntryByKey(keyOfOldestItem);
        }
    };

    self.set = function (key, value, maxAge) {
        if (cache[key]) {
            cache[key].value = value;
            cache[key].setAt = new Date();
            cache[key].maxAge = maxAge || cache[key].maxAge;
        } else {
            if (options.maxEntries && entries >= options.maxEntries) {
                deleteOldestEntry();
            }

            cache[key] = {
                value: value,
                setAt: new Date(),
                maxAge: maxAge
            };

            entries++;
        }
    };

    self.get = function (key) {
        var entry = cache[key];

        if (entry) {
            if (entry.maxAge && entry.maxAge <= ageOf(entry)) {
                deleteEntryByKey(key);
            }

            return entry.value;
        }
    };
};
