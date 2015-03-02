#Crossroads.net Clientside Application

The client facing website for crossroads church. 

###Getting Started 
The only thing you'll need to get started is NodeJS. Head over to [http://nodejs.org/](http://nodejs.org) and install based on your operating system. We use npm scripts to run webpack so there are no node modules to install globally. Once you pull down the code, just run `npm i` to install all dependencies locally. 

###Configuration
By default webpack inserts `http://localhost:49380` everywhere it finds `__API_ENDPOINT__` in the javascript. This can be changed by creating and setting an environment variable called **CRDS_API_ENDPOINT**. 

For windows users:
``` set CRDS_API_ENDPOINT = http://path-to-api-host/ ```

Mac and Linux:
``` export CRDS_API_ENDPOINT = http://path-to-api-host/ ```

>Keep in mind that this way of setting environment variables will not be persistent, windows users will have to add this variable in system settings and linux/mac users will have to set it in thier .bashrc/.zshrc files for persistence. 

###Build
To just build the project, run `npm run build`

###Run
To run the project, run `npm start` and point your browser to `http://localhost:8080`. If you want live reload, use `http://localhost:8080/webpack-dev-server` but keep in mind that the anglar inspector will not work correctly and routes will not show up correctly with live reload. 

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