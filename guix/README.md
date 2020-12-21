# Jupyter Notebook Dependencies

Although GitHub renders Jupyter notebooks, in order to be able to use the
interactive portions or to run/modify the code yourself, you will need the
python dependencies.

Jupyter is available in most Linux distributions as well as a variety of
installers.

## Guix

Guix is a package manager that can be used to provide the dependencies using
the supplied `manifest.scm` and `channel-specs.scm` files.

Guix can be [easily
installed](https://guix.gnu.org/manual/en/html_node/Binary-Installation.html)
on top of most Linux distributions, either manually or using the automated
installation script.

With the `manifest.scm`, `guix` will be able create a shell environment with
all the dependencies installed and run `jupyter notebook` inside of it:

```sh
guix environment --pure --manifest=guix/manifest.scm -- jupyter notebook
```

In order to use the frozen dependencies, you can use the supplied channel
specification, which is a snapshot of a known working set of package versions,
you can use the `channel-specs.scm` file [to obtain a guix
profile](https://guix.gnu.org/blog/2019/guix-profiles-in-practice/) with exact
dependencies used to generate the outputs in the notebook.

## Debian/Ubuntu

The following should work using more recent releases (untested):

```sh
apt install jupyter python3-{matplotlib,attr,typing-extensions,mypy}
```

Unfortunately the supplied `pandas` and `ipywidgets` versions are too old in
Debian 10, so it seems the best approach is to install everything through pip:

```sh
pip3 install --user --ignore-installed ipywidgets pandas matplotlib attr mypy
```

Note that when using the `--user` option the `jupyter` executable will be in
`~/.local/bin`.


## Fedora

```sh
dnf install python3-{notebook,pandas,matplotlib,attrs,mypy,typing-extensions}
```

`ipywidgets` is not in Fedora, but can be installed with pip:

```sh
pip3 install --user ipywidgets
```

## `ipywidgets` Notebook Extension

After installing the python dependencies ensure that the widgets extension is
enabled in Jupyter itself:

```sh
jupyter nbextension list
```

and enable it if not:

```sh
jupyter nbextension enable --py widgetsnbextension
```

which should load the required JavaScript for the interactive plots to work.
