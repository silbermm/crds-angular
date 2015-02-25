#Crossroads.net Clientside Application

The client facing website for crossroads church. 

###Configuration
By default webpack inserts `http://localhost:49380` everywhere it finds `__API_ENDPOINT__` in the javascript. This can be changed by creating and setting an environment variable called CRDS_API_ENDPOINT. 

For windows users:
``` set API_ENDPOINT = http://some-other-url/path-to-api/ ```

Mac and Linux:
``` export API_ENDPOINT = http://some-other-url/path-to-api/ ```

>Keep in mind that this way of setting environment variables will not be persistant, windows users will have to add this variable in system settings and linux/mac users will have to set it in thier .bashrc/.zshrc files for persistance. 

###Build
To just build the project, run `npm run build`

###Run
To run the project, run `npm start` and point your browser to `http://localhost:8080`. If you want live reload, use `http://localhost:8080/webpack-dev-server` but keep in mind that the anglar inspector will not work correctly and routes will not show up correctly with live reload. 

