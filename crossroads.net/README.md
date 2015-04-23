#Crossroads.net Clientside Application

The client facing website for crossroads church. 

###Getting Started 
The first thing you'll need to get started is NodeJS. Head over to [http://nodejs.org/](http://nodejs.org) and install based on your operating system. Once you pull down the code, just run `npm i` to install all dependencies locally.

We use gulp scripts to build and run webpack so you will also need to install gulp globally.  This is not required, but makes command-line tasks easier later on.  To install gulp globally, use one of the following commands.  Both will install gulp into the NodeJS path, which is presumably already on your OS's execution PATH.

For Windows users (replace the prefix value below with the path to your NodeJS install):
``` npm set prefix "C:\Program Files\nodejs" ```
``` npm install -g gulp ```

Mac and Linux (replace the prefix value below with the path to your NodeJS install):
``` npm set prefix /usr/local/nodejs ```
``` npm install -g gulp ```

###Configuration
By default webpack inserts `http://localhost:49380` everywhere it finds `__API_ENDPOINT__` in the javascript. This can be changed by creating and setting an environment variable called **CRDS_API_ENDPOINT**. 
By default webpack inserts `http://content.crossroads.net` everywhere it finds `__CMS_ENDPOINT__` in the javascript. This can be changed by creating and setting an environment variable called **CRDS_CMS_ENDPOINT**. 

For windows users:
``` set CRDS_API_ENDPOINT = http://path-to-api-host/ ```
``` set CRDS_CMS_ENDPOINT = http://path-to-content-host/ ```

Mac and Linux:
``` export CRDS_API_ENDPOINT = http://path-to-api-host/ ```
``` export CRDS_CMS_ENDPOINT = http://path-to-content-host/ ```

>Keep in mind that this way of setting environment variables will not be persistent, windows users will have to add this variable in system settings and linux/mac users will have to set it in their .bashrc/.zshrc files for persistence. 

###Build
To just build the project, run `gulp build-dev` for a dev build, or `gulp build` for production.

###Test
There are two types of tests available, Unit Tests and Functional Tests. 
#### Unit Tests
We use karma as our test runner and Jasmine to write the specs. You will need to install karma globablly to run the tests. 

Unit tests are kept in the (specs)[./specs] folder.

Windows users can run:
``` npm set prefix "C:\Program Files\nodejs" ```

``` npm install -g karma ```

Mac and Linux users can run:
``` npm install -g karma ```

Once karma is installed, just run `karma start crossroads.conf.js` which will open chrome and run the tests. Click the debug button to see the results. Refreshing this page will re-run the tests.

#### Functional Tests
We use protractor to run the tests and Jasmine to write the specs. You will need to install protractor globally. 

Functional Tests are kept in (e2e)[./e2e]

Windows users can run:
``` npm set prefix "C:\Program Files\nodejs" ```

``` npm install -g protractor ```

Mac and Linux users can run:
``` npm install -g protractor ```

Next, update the selenium drivers. `webdriver-manager update --out_dir=node_modules/protractor/selenium`. 

To run tests in safari, you will need to download the safari plugin from (here)[http://selenium-release.storage.googleapis.com/index.html?path=2.45/] and install it.

You will now be able to run protractor by typing `protractor protractor.conf.js`. 



###Run
To run the project, run `gulp start` and point your browser to `http://localhost:8080`. If you want live reload, use `http://localhost:8080/webpack-dev-server` but keep in mind that the angular inspector will not work correctly and routes will not show up correctly with live reload. 

##Mac OS with Gateway code running under VirtualBox Windows Guest
Follow these instructions in order to setup the application to call Gateway services that are hosted on IIS Express on a Windows VM running under VirtualBox on a Mac.

###Create a VirtualBox Host-Only Network
1. Open VirtualBox
2. Navigate to **VirtualBox > Preferences...**
3. Click on **Network**
4. Click on **Host-Only Networks**
5. Click on the little network card with a plus sign on the right of the list
6. This will add an entry in the list called `vboxnet0`
7. Click **OK**

###Add a second adapter to VM
1. From VM VirtualBox Manager, with VM powered off, select VM and click **Settings**
2. Click **Network**
3. Click **Adapter 2**
4. Check **Enable Network Adapter**
5. Under **Attached to:** select `Host-only Adapter`
6. Under **Name** select `vboxnet0`
7. Click **OK**

###Start Windows VM and configure IIS Express
1. Open File Explorer and goto C:\Users\ *username*\Documents\IISExpress\config
2. Edit `applicationhost.config`
3. Find a `<site>` entry under `<sites>` that matches the name of the Visual Studio Project (e.g. `crds-angular`)
4. Under the `<bindings>` entry add `<binding protocol="http" bindingInformation="*:`[port number of IIS entry point, most likely `49380` ]`:`[name of VM]`"/>`, e.g. `<binding protocol="http" bindingInformation="*:49380:silbervm"/>`
5. Save file and start Visual Studio
6. Load and run the solution
7. Look in the task bar for the IIS Express icon and right click it
8. Look for an entry under **View Sites** that matches the name of the `<site>` key above and click on it.
9. You should see the binding you added represented as a url, e.g. `http://silbervm:49380/` under **Browse Applications**

###Configure OS X to use network
1. Edit `/etc/hosts` as *root*
2. On the Windows VM, **Open Network and Sharing Center** and click on **Local Area Connection 2**
3. Click **Details...**
4. Note the `IPv4 Address`, most likely `192.168.56.101`
5. Back on OS X, create a entry in `/etc/hosts` using the VM name as the DNS name, e.g. `192.168.56.101  silbervm`
6. As described above, add the **CRDS_API_ENDPOINT** environment variable to match the configuration, e.g. `export  CRDS_API_ENDPOINT=http://silbervm:49380/`

##Folder Naming Convention
1. Use descriptive folder names
2. Seperate multiple words with underscords (i.e. 'sign_up_to_serve')

##Angular Style Guide
We will follow the [Crossroads Angular Style Guide](https://github.com/crdschurch/angular-styleguide).
