﻿Npm 1.4.15.2
============

Npm is a package manager for Node.js.


Installation overview
---------------------

Npm cmd is installed into .bin dir in your project, along with Node.js cmd. NuGet rules are
the same: everything deployed by NuGet, so use package restore and ignore packages in VCS.

Git is not needed to be installed on dev machines or build servers, as long as your're not
using Git URLs for dependencies which using Git submodules (very rare case).


Automation
----------

Use ".bin\npm" command to run Npm in your build scripts. For example, here is a simple
MsBuild target to restore Npm packages defined in package.json:

<Target Name="NpmInstall">
  <Exec Command=".bin\npm install" />
</Target>


Daily usage
-----------

Because ".bin" was added to your PATH, you should be able to run Npm directly in the
command prompt from the project dir. For example, if "MySite.Web" is a project dir:

D:\Projects\MySite\MySite.Web> npm install lodash

Note: add ".bin" to the PATH manually for other developers in your team.
Note: if PATH was changed, restart your command prompt to refresh environment variables.


Proxy settings
--------------

If Npm should use a proxy for remote connections, set 'HTTP_PROXY' and/or 'HTTPS_PROXY'
environment variables to the proxy URL. For Node.js delivered via NuGet, edit
"~/.bin/node.cmd" file:
    
    SET HTTP_PROXY=http://1:1@127.0.0.1:8888
    SET HTTPS_PROXY=http://1:1@127.0.0.1:8888
    
where "http://1:1@127.0.0.1:8888" is the proxy at "127.0.0.1:8888" with username "1" and
password "1" used for authentication.


Docs
----

Read docs abput Npm at https://www.npmjs.org/doc/


------------------------------------------------------
© 2014 npm, Inc. and Contributors