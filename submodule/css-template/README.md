# css-template

1.  start project(first init):

- download and install Node.js (https://nodejs.org/uk/download/current)
- cd FOLDERNAME
- npm git clone https://gitlab.com/system-m/css-template.git
- npm install --global gulp ////\*if it isn`t on computer
- npm i

2. use previously added theme

- run 2 gulp tasks using selected theme
- tasks of previously installed theme(you can find it in gulpfile.js):
  - gulp build-dark-blue
  - gulp build-dark-grey
  - gulp build-mis
  - gulp build-diklz

2.1 add new custom theme

- add new folder to less/themes
- copy default theme(default_cost) to your theme folder
- edit palette.less & variables.less & custom_icons according your needs
- include path to your theme in gulpconfig.js (ex. themeDefaultCost)
- create task for your theme in gulpfile.js (ex. 'build-default')
- run 2 gulp tasks using your custom task instead of default

2.2 use default themes:

- "gulp build-default" - start task to set default themes
  if you need :
- start tasks (just for transfer fonts, images, libs) => "gulp transfer" ['img', 'fonts' , 'js', 'css', 'jsLibs', 'html'];
- build project without mapping && with cleaning css && js => "gulp build" ['img', 'fonts' , 'cssClear', 'jsClear', 'jsLibs', 'html'];
- start tasks (default with watching and server without cleaning .css && .js) => "gulp" ['css', 'js', 'html', 'serve', 'watch'];

3.  update css&js:

- watching => "gulp" - start tasks (default with watching and server without cleaning .css && .js): ['css', 'js', 'html', 'serve', 'watch'];
- transfer => "gulp transfer" - start tasks (just for transfer fonts, images, libs): ['img', 'fonts' , 'js', 'css', 'jsLibs', 'html'];
- prod build => "gulp build" - start tasks (build project without mapping && with cleaning css && js): ['img', 'fonts' , 'cssClear', 'jsClear', 'jsLibs', 'html'];

###general rules:

- images - prefer **.png || **.svg
- after adding file(**.html, **.less, \*\*.js) - restart project
- after adding images to ./img, \*\*.js to ./libs, fonts - run gulp-transfer
- before release run gulp-build (delete or comment test files before running)

###VS Code rules:

- install PostCSS Sorting extension
- PostCSS settings:
  {
  "postcssSorting.config": {
  "properties-order": [

              "content",
              "quotes",


            "display",
            "visibility",


            "position",
            "z-index",
            "top",
            "right",
            "bottom",
            "left",


            "box-sizing",


            "grid",
            "grid-after",
            "grid-area",
            "grid-auto-columns",
            "grid-auto-flow",
            "grid-auto-rows",
            "grid-before",
            "grid-column",
            "grid-column-end",
            "grid-column-gap",
            "grid-column-start",
            "grid-columns",
            "grid-end",
            "grid-gap",
            "grid-row",
            "grid-row-end",
            "grid-row-gap",
            "grid-row-start",
            "grid-rows",
            "grid-start",
            "grid-template",
            "grid-template-areas",
            "grid-template-columns",
            "grid-template-rows",


            "flex",
            "flex-basis",
            "flex-direction",
            "flex-flow",
            "flex-grow",
            "flex-shrink",
            "flex-wrap",
            "align-content",
            "align-items",
            "align-self",
            "justify-content",
            "order",


            "width",
            "min-width",
            "max-width",
            "height",
            "min-height",
            "max-height",


            "margin",
            "margin-top",
            "margin-right",
            "margin-bottom",
            "margin-left",


            "padding",
            "padding-top",
            "padding-right",
            "padding-bottom",
            "padding-left",


            "float",
            "clear",


            "overflow",
            "overflow-x",
            "overflow-y",


            "clip",
            "zoom",


            "columns",
            "column-gap",
            "column-fill",
            "column-rule",
            "column-span",
            "column-count",
            "column-width",


            "table-layout",
            "empty-cells",
            "caption-side",
            "border-spacing",
            "border-collapse",
            "list-style",
            "list-style-position",
            "list-style-type",
            "list-style-image",


            "transform",
            "transform-origin",
            "transform-style",
            "backface-visibility",
            "perspective",
            "perspective-origin",


            "transition",
            "transition-property",
            "transition-duration",
            "transition-timing-function",
            "transition-delay",


            "animation",
            "animation-name",
            "animation-duration",
            "animation-play-state",
            "animation-timing-function",
            "animation-delay",
            "animation-iteration-count",
            "animation-direction",


            "border",
            "border-top",
            "border-right",
            "border-bottom",
            "border-left",
            "border-width",
            "border-top-width",
            "border-right-width",
            "border-bottom-width",
            "border-left-width",


            "border-style",
            "border-top-style",
            "border-right-style",
            "border-bottom-style",
            "border-left-style",


            "border-radius",
            "border-top-left-radius",
            "border-top-right-radius",
            "border-bottom-left-radius",
            "border-bottom-right-radius",


            "border-color",
            "border-top-color",
            "border-right-color",
            "border-bottom-color",
            "border-left-color",


            "outline",
            "outline-color",
            "outline-offset",
            "outline-style",
            "outline-width",


            "stroke-width",
            "stroke-linecap",
            "stroke-dasharray",
            "stroke-dashoffset",
            "stroke",


            "opacity",


            "background",
            "background-color",
            "background-image",
            "background-repeat",
            "background-position",
            "background-size",
            "box-shadow",
            "fill",


            "color",


            "font",
            "font-family",
            "font-size",
            "font-size-adjust",
            "font-stretch",
            "font-effect",
            "font-style",
            "font-variant",
            "font-weight",


            "font-emphasize",
            "font-emphasize-position",
            "font-emphasize-style",


            "letter-spacing",
            "line-height",
            "list-style",
            "word-spacing",


            "text-align",
            "text-align-last",
            "text-decoration",
            "text-indent",
            "text-justify",
            "text-overflow",
            "text-overflow-ellipsis",
            "text-overflow-mode",
            "text-rendering",
            "text-outline",
            "text-shadow",
            "text-transform",
            "text-wrap",
            "word-wrap",
            "word-break",


            "text-emphasis",
            "text-emphasis-color",
            "text-emphasis-style",
            "text-emphasis-position",


            "vertical-align",
            "white-space",
            "word-spacing",
            "hyphens",


            "src",


            "tab-size",
            "counter-reset",
            "counter-increment",
            "resize",
            "cursor",
            "pointer-events",
            "speak",
            "user-select",
            "nav-index",
            "nav-up",
            "nav-right",
            "nav-down",
            "nav-left"
        ]
    }

}
