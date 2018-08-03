#!/usr/bin/python
#coding=utf-8

import os,sys
import re

os.chdir(os.path.abspath(sys.path[0]))
reload(sys)
sys.setdefaultencoding('utf-8')

ExportCSFilePath = "../Server/Server/Socket/MsgCode.cs"

MsgPattern = None

MsgName_List = []
MsgCode_List = []

def GetMsgPattern():
	global MsgPattern
	if not MsgPattern:
		MsgPattern = re.compile(r'\s*message\s*(([A-Z][A-Z])([a-zA-Z]*))\s*')
	pass
	return MsgPattern
pass

def CheckLine(file_name, line):
	pattern = GetMsgPattern()
	results = pattern.findall(line.decode('utf-8'))
	if len(results) >= 1:
		# ST为自定义数据体开头标志
		if results[0][1] != "ST":
			print file_name, results
			msg_name = results[0][1] + results[0][2]
			msg_code = results[0][1] + "_" + results[0][2]
			MsgName_List.append(msg_name)
			MsgCode_List.append(msg_code)
		pass
	pass
pass

def FindAllMsgName():
	for root_path, dirs, files in os.walk("."):
		for file in files:
			file_path = "%s%s%s" % (root_path, os.sep, file)
			str_list = os.path.splitext(file)
			file_name = str_list[0]
			file_format = str_list[1]

			if file_format == ".proto":
				file_stream = open(file_path, 'r')
				lines = file_stream.readlines()
				for line in lines:
					CheckLine(file_name, line)
				pass
			pass
		pass
	pass
pass

def ExportCSMsgCode():
	stream = open(ExportCSFilePath, 'w+')

	line = "\
using Common.Protobuf;\n\
using Google.Protobuf;\n\
using System;\n\
using System.Collections.Generic;\n\
\n\
class MsgCode\n\
{\n\
"
	stream.write(line)

	# 协议号分配
	cur_msg_id = 10000
	for msg_code in MsgCode_List:
		cur_msg_id = cur_msg_id + 1
		stream.write("\tpublic const short %s = %d;\n" % (msg_code, cur_msg_id))
	pass

	# 协议号对应的协议解析类
	line = "\tpublic static Dictionary<short, MessageParser> ProtocolParser = new Dictionary<short, MessageParser>() {\n"
	stream.write("\n" + line)

	for i in xrange(0, len(MsgCode_List) - 1):
		stream.write("\t\t{MsgCode.%s, %s.Parser},\n" % (MsgCode_List[i], MsgName_List[i]))
	pass

	line = "\t};\n"
	stream.write(line)

	# 协议对应的协议号
	line = "\tpublic static Dictionary<Type, short> ProtocolMap = new Dictionary<Type, short>(){\n"
	stream.write("\n" + line)

	for i in xrange(0, len(MsgCode_List) - 1):
		stream.write("\t\t{typeof(%s), MsgCode.%s},\n" % (MsgName_List[i], MsgCode_List[i]))
	pass

	line = "\t};\n"
	stream.write(line)

	line = "};\n"
	stream.write(line)

	stream.close()
pass

if __name__ == "__main__":
	FindAllMsgName()
	ExportCSMsgCode()
pass
