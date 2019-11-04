# css-template

##start project:
*   cd FOLDERNAME
<!-- *   npm git clone https://gitlab.com/system-m/css-template.git -->
*   npm install --global gulp ////*if it isn`t on computer
*   npm i

##scripts:
*   "gulp" - start tasks (default with watching and server without cleaning .css && .js): ['css', 'js', 'html', 'serve', 'watch'];
*   "npm run gulp-transfer" - start tasks (just for transfer fonts, images, libs):  ['img', 'fonts' , 'js', 'css', 'jsLibs', 'html'];
*   "npm run gulp-arestenko" - synchronization projects - !!!use very carefully or not use - it`d your the best decision
*   "npm run gulp-build" - start tasks (build project without mapping && with cleaning css && js):  ['img', 'fonts' , 'cssClear', 'jsClear', 'jsLibs', 'html'];

###general rules:

*   images - prefer **.png || **.svg
*   after adding file(**.html, **.less, **.js) - restart project
*   after adding images to ./img, **.js to ./libs, fonts - run gulp-transfer
*   before release run gulp-build (delete or comment test files before running)