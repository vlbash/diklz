describe('libversion', function () {
    it('lib version should be 1.3', function () {
        var lib = new UacPlugin("http://cala.it-engineering.com.ua/service/");
        expect(lib.version).toBe('1.3');
    });
});