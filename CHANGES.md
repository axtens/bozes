### **Changes**

***2019-11-06*** The custom tool's command line now reads `...\bozes.exe -f %f -d %d -s %s -p %p -n %n -D %D -t %t -T %T` and the logic has been updated to take `-t` as the value of the selected text.

***2019-11-06*** Commandline parsing has changed. The Dashed items are now a list of string, allowing for multiple instances of things. This is totally useless in the context of this plugin.