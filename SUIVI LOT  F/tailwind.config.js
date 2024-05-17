module.exports = {
    darkMode: "class",
    content: [
        './Pages/**/*.cshtml',
        './Views/**/*.cshtml',
        './wwwroot/**/*.js',
        './wwwroot/js/*.js',
        './wwwroot/js/Preview.js',
        '/wwwroot/js/Preview.js',
        'wwwroot/js/Preview.js',
        '../wwwroot/js/Preview.js',
    ],
    theme: {
        container: {
            center: true,
        },
        extend: {
            display: {
                '-webkit-box': '-webkit-box',
            },
        },
    },
    corePlugins: {
        preflight: false,
    },

}