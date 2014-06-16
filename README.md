#Dict v1.0
####Serializable Dicitonary

Copyright (C) 2014 Skywatch Entretenimento Digital LTDA - ME
This is free software. Please refer to LICENSE for more information.

##Instructions
--------------

Installation and use instructions available at: http://www.skywatch.com.br/dict

##Known Issues
--------------
If you duplicate an object holding a Dict reference, the new instance will keep a reference to the old Dict object. This has to do with Unity's ScriptableObject behaviour. To duplicate the Dict itself, please do:

```Dict newInstance = (Dict)Instantiate(oldInstance);```

##Changelog
--------------
- v1.0: Initial Release