# OrgChart
A visual representation for an org chart

The PopOrgChart folder contains the C# program that will read in user objects from the OU you point it at and spit out a hierarchy of those objects in a json file.  If you don't have your manager attribute set properly you will get strange results.

The orgchart folder is an example of the front end for the visualization.  It reads in the json file generated from the poporgchart application and visualizes it with the awesome Javascript visualization library by Nicolas Garcia Belmonte (http://philogb.github.com/).

You can change some of the visualization and animation properties by editing the visual.js file.
