mergeInto(LibraryManager.library, {
  HelloWorld: function () {
    window.alert("Hello, world!");
  },

  LocalStorageSet: function(keyName, valueName) {
    console.log(`LocalStorageSet(${UTF8ToString(keyName)}, ${UTF8ToString(valueName)})`)
    window.localStorage.setItem(UTF8ToString(keyName), UTF8ToString(valueName));
  },

  LocalStorageGet: function(keyName) {
    console.log(`LocalStorageGet(${UTF8ToString(keyName)})`)
    let item = window.localStorage.getItem(UTF8ToString(keyName));
    console.log(`LocalStorageGet result: (${item})`)

    var buffer = null;

    if (item === null) {
      buffer = _malloc(1);
      buffer[0] = 0;
      return buffer;
    }

    const length = lengthBytesUTF8(item);
    buffer = _malloc(length + 1);
    stringToUTF8(item, buffer, length);
    return buffer;
  },

  LocalStorageRemove: function(keyName) {
    window.localStorage.removeItem(UTF8ToString(keyName));
  },

  LocalStorageClear: function() {
    window.localStorage.clear();
  }
});
