# Design Documentation

The `.plantuml` files are PlantUML files. To see these images, it's easiest to
install the PlantUML extension for Visual Studio Code, and then render within
VSCode.

These files are textual representation of the implementation, that are manually
rendered, so that other Markdown files can reference the images.

The reason why the results are rendered, as not all systems support
automatically rendering PlantUML (e.g. GitHub), and Visual Studio Code Markdown
Preview can't render files when referenced externally (the PlantUML must be in
the markdown file).

## Generating the Documentation

This workflow is based on Visual Studio Code

* Install the plantuml extension in Visual Studio Code.
* (Optional) Install the plantuml server. I used a docker container described at
  [GitHub PlantUML Server](https://github.com/plantuml/plantuml-server) on Linux
  (while Visual Studio Code was running on Windows).
  * Be sure to configure the PlantUML extension on Visual Studio code to
    reference the server, such as `http:/192.168.1.150:8080`.

During writing the documentation, Visual Studio Code can render the PlantUML in
real-time with `Alt-D`. When satisfied, I use the command to export the current
diagram to disk, and place it in the `out` folder, which the Markdown file then
refers to.

This way, it's possible to get a render of the document at the commit that is of
interest (not a proxy solution as some suggest that can only pull in the `HEAD`,
which is more dangerous than convenient), and it works in a large number of
environments.
