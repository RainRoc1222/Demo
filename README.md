<h1 align="center">使用方式</h1>

<h2>選擇通訊協定</h2>
點擊左上角的☰符號展開Menu，從TCP/IP、SerialPort、Modubus選擇一個通訊協定。

<h2>TCP/IP</h2>
輸入IP、Port，並按下Save儲存。  
<image src=""/>
選擇Server/Client，並按下Connect開啟通訊。  
<image src=""/>
打開Sscom，輸入相同的IP、Port，並選擇Server/Client，按下Listen/Connect與程式做對接。
<image src=""/>
輸入預傳送的訊息，並按下Send。    
<image src=""/>
Sscom的Server/Client接受到訊息。
<image src=""/>
在Sscom中輸入預傳送的訊息，並按下Send。
<image src=""/>
應用程式接收到Server/Client回傳訊息。 
<image src=""/>   
---

<h2>SerialPort</h2>
選擇PortName、BaudRate、Parity、DataBits、StopBits，並按下Save儲存。  
<image src=""/>
按下Connect開啟通訊。  
<image src=""/>
打開Virtual Serial Port Driver，建立虛擬COM Port。   
   <image src=""/>
打開Sscom，輸入相同的IP、Port，並選擇Server/Client，按下Listen/Connect與程式做對接。
<image src=""/>
輸入預傳送的訊息，並按下Send。    
<image src=""/>
Sscom的Server/Client接受到訊息。
<image src=""/>
在Sscom中輸入預傳送的訊息，並按下Send。
<image src=""/>
應用程式接收到Server/Client回傳訊息。 
<image src=""/>  

Here are some READMEs generated using common-readme:

- [`collide-2d-aabb-aabb`](https://github.com/noffle/collide-2d-aabb-aabb)
- [`goertzel`](https://github.com/noffle/goertzel)
- [`twitter-kv`](https://github.com/noffle/twitter-kv)

*([Submit a pull request](https://github.com/noffle/common-readme/pulls) and add
yours here!)*

## Usage

With [npm](https://npmjs.org/) installed, run

    $ npm install -g common-readme

`common-readme` is a command line program. You run it when you've started a new
module that has a `package.json` set up.

When run, a brand new README is generated and written to your terminal. You can
redirect this to `README.md` and use it as a basis for your new module.

    $ common-readme > README.md

This brand new readme will be automatically populated with values from
`package.json` such as `name`, `description`, and `license`. Stub sections will
be created for everything else (Usage, API, etc), ready for you to fill in.

## Why?

This isn't a crazy new idea. Other ecosystems like [Perl's
CPAN](http://perldoc.perl.org/perlmodstyle.html) have been benefiting from a
common readme format for years. Furthermore:

1. The node community is powered by us people and the modules we share. It's our
   documentation that links us together. Our README is the first thing
   developers see and it should be maximally effective at communicating its
   purpose and function.

2. There is much wisdom to be found from the many developers who have written
   many many modules. Common readme aims to distill that experience into a
   common format that stands to benefit us all; especially newer developers!

3. Writing the same boilerplate is a waste of every author's time -- we might as
   well generate the common pieces and let the author focus on the content.

4. Scanning through modules on npm is a part of every node developer's regular
   development cycle. Having a consistent format lets the brain focus on content
   instead of structure.

## The Art of README

For even more background, wisdom, and ideas, take a look at the article that
inspired common-readme:

- [*Art of README*](https://github.com/noffle/art-of-readme).

## Install

With [npm](https://npmjs.org/) installed, run

```shell
npm install -g common-readme
```

You can now execute the `common-readme` command.

## Acknowledgments

A standard readme format for the Node community isn't a new idea. Inspiration
came from many conversations and unrealized efforts in the community:

- <https://github.com/feross/standard/issues/141>
- [richardlitt/standard-readme](https://github.com/RichardLitt/readme-standard)
- [zwei/standard-readme](https://github.com/zcei/standard-readme)

This, in addition to my own experiences evaluating hundreds of node modules and
their READMEs.

I was partly inspired by the audacity of the honey-badger-don't-care efforts of
[standard](https://github.com/feross/standard).

I also did a great deal of Perl archaeology -- it turns out the monks of the
Perl community already did much of the hard work of [figuring out great
READMEs](http://perldoc.perl.org/perlmodstyle.html) and the wisdom around small
module development well over a decade ago.

Thanks to @mafintosh, @andrewosh, and @feross for many long conversations about
readmes and Node.

## See Also

READMEs love [`readme`](https://www.npmjs.com/package/readme)!

## License

ISC
